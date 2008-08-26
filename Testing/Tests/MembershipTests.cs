using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using System.Data.SqlClient;
using System.Web.Profile;

namespace Testing.Tests
{
    class MembershipTests: AbstractTest
    {
        string contactIdKey = "CDM_UserId";

        public string ResetPassword(string username)
        {
            SqlConnection conn = OpenConnection();
            Console.WriteLine(Membership.ApplicationName);
            MembershipUserCollection users = Membership.GetAllUsers();
            //MembershipUserCollection users = Membership.FindUsersByEmail(userName);
            if (users.Count == 1)
                return users[username].ResetPassword();
            else
                return null;
        }

        public void CreateNewUser(string username, string email, 
            string password, string cdm_id)
        {
            MembershipUser user = Membership.CreateUser(username, password, email);
            Membership.UpdateUser(user);
            ProfileBase profile = ProfileBase.Create(user.UserName);
            profile.SetPropertyValue(contactIdKey, cdm_id);
            profile.Save();
        }

        public void DeleteUser(string username)
        {
            Membership.DeleteUser(username);
        }

        public int UsersCount()
        {
            return Membership.GetAllUsers().Count;
        }

    }
}
