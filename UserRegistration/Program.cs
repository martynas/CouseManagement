using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web.Profile;
using System.Net.Mail;

namespace Atrendia.CourseManagement.UserRegistration
{
    class Program
    {
        string contactIdKey = "CDM_UserId";
        StreamWriter logWriter;
        Logic.COMEndpoint.COMEndpoint currentEndpoint;

        static void Main(string[] args)
        {
            new Program().Run();
        }

        private Program()
        {
            string applicationName =
                        ConfigurationManager.AppSettings[Helpers.Endpoint.applicationNameKey];
            currentEndpoint = new Logic.COMEndpoint.COMEndpoint(applicationName);

            OpenLog();
        }

        #region Registration of Client Project Managers
        public void Run()
        {
            registerProjectManagers();
            registerTrainers();
        }

        protected void registerTrainers()
        {
            Log("Registering trainers");
            ensureRole(Helpers.UserRole.CDMTrainer);

            List<Logic.Entities.User> users = currentEndpoint.
                GetCDMUsersInRole(Logic.ContactRoles.Trainer);
            Log(string.Format("There are {0} users in CDM database having trainer's role",
                users.Count));

            int added = 0;
            foreach (Logic.Entities.User cdmuser in users)
            {
                MembershipUser user;
                string password;
                if (registerUser(cdmuser.Email, cdmuser.Id, Helpers.UserRole.CDMTrainer, out user, out password))
                {
                    sendEmail(cdmuser.Email, cdmuser.ToString(), password,
                        Resources.Email_Subject, Resources.Email_Text);
                    added++;
                }
            }

            Log(string.Format("{0} trainers added", added));
            Log("Finished");
        }

        /// <summary>
        /// Registers clients with newlly asssigned PM's role
        /// </summary>
        protected void registerProjectManagers()
        {
            Log("Registering client project managers");
            ensureRole(Helpers.UserRole.ClientPM);

            List<Logic.Entities.Contact> contacts = currentEndpoint.
                GetContactsInRole(Logic.ContactRoles.ProjectManager);
            Log(string.Format("There are {0} contacts in CDM database having project manager's role", 
                contacts.Count));

            int added = 0;
            foreach (Logic.Entities.Contact contact in contacts)
            {
                MembershipUser user;
                string password;
                if (registerUser(contact.Email, contact.Id, Helpers.UserRole.ClientPM, out user, out password))
                {
                    sendEmail(contact.Email, contact.ToString(), password,
                        Resources.Email_Subject, Resources.Email_Text);
                    added++;
                }
            }

            Log(string.Format("{0} users added", added));
            Log("Finished");
        }

        /// <summary>
        /// Registers user at CDM Course Management System
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cdmUserId"></param>
        /// <param name="role"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool registerUser(string email, string cdmUserId, string role, 
            out MembershipUser user, out string password)
        {
            user = Membership.GetUser(email);
            if (user == null)
            {
                password = GeneratePassword();
                Log(string.Format("({0}) {1}, {2}", cdmUserId, email, role));
                Log(string.Format("Assigned password: {0}", password));
                // Registering new user
                try
                {
                    user = Membership.CreateUser(email, password, email);
                    user.Comment = string.Format("Imported at {0}", DateTime.UtcNow);
                    Membership.UpdateUser(user);
                    Roles.AddUserToRole(email, role);

                    ProfileBase profile = ProfileBase.Create(user.UserName);
                    profile.SetPropertyValue(contactIdKey, cdmUserId);
                    profile.Save();
                }
                catch (Exception ex)
                {
                    Log("Failed to register:");
                    Log(ex);
                    Log(String.Format("Username: {0}, CDMUserId: {1}, role: {2}", email, cdmUserId, role));
                    user = null;
                    password = null;
                    return false;
                }
                return true;
            }
            else
            {
                if (!Roles.IsUserInRole(email, role))
                    Roles.AddUserToRole(email, role);
                password = null;
                return false;
            }
        }

        void ensureRole(string role)
        {
            if (!Roles.RoleExists(role))
                Roles.CreateRole(role);
        }

        /// <summary>
        /// Sends login credentials to a new user of Course Management system
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>
        bool sendEmail(string email, string name, string password, string emailSubject, string emailBody)
        {
            // Sending login credentials to the user by e-mail
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(new MailAddress(email, name));
                message.From = new MailAddress(ConfigurationManager.AppSettings["Email_From"]);
                message.Subject = emailSubject;
                message.Body = string.Format(emailBody, name, email, password);
                message.IsBodyHtml = false;
                SmtpClient client = new SmtpClient();
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Log("Failed to send email:");
                Log(ex);
            }
            return false;
        }

        #endregion

        #region General
        /// <summary>
        /// Generates random password;
        /// </summary>
        /// <returns></returns>
        private string GeneratePassword()
        {
            int length = 5;
            string s = string.Empty;
            Random random = new Random();
            while (length-- > 0)
            {
                s += (char)('0' + random.Next(10));
            }
            return s;
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message"></param>
        private void Log(string message)
        {
            logWriter.WriteLine("{0:HH:mm:ss}: {1}", DateTime.UtcNow, message);
        }

        /// <summary>
        /// Log exception to log.
        /// </summary>
        /// <param name="ex"></param>
        private void Log(Exception ex)
        {
            Log(string.Format("Fatal exception: {0}", ex.GetType().FullName));
            Log(ex.Message);
            Log(ex.StackTrace);
        }

        /// <summary>
        /// Open connection to the server.
        /// </summary>
        /// <returns></returns>
        private SqlConnection OpenConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["Membership_DB"];
            SqlConnection conn = new SqlConnection(settings.ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Open log file.
        /// </summary>
        private void OpenLog()
        {
            string fileName = string.Format("{0:yyyy_MM_dd_HH_mm_ss}.log", DateTime.UtcNow);
            string directory = ConfigurationManager.AppSettings["Log_DIR"];
            logWriter = new StreamWriter(Path.Combine(directory, fileName));
            logWriter.AutoFlush = true;
        }
        #endregion
    }
}
