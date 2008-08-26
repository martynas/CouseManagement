///
/// Helpers for the frontend application
///

using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace Atrendia.CourseManagement.Helpers
{
    public class Endpoint
    {
        /// <summary>
        /// Returns per-request endpoint.
        /// </summary>
        static string endpointKey = "CDM_Endpoint";
        public static string applicationNameKey = "CDM_ApplicationName";
        public static Logic.IEndpoint CurrentEndpoint
        {
            get
            {
                IDictionary items = HttpContext.Current.Items;
                if (!items.Contains(endpointKey))
                {
                    string applicationName =
                        ConfigurationManager.AppSettings[applicationNameKey];
                    items[endpointKey] = new Logic.COMEndpoint(applicationName);
                }
                return (Logic.IEndpoint)items[endpointKey];
            }
        }

        /// <summary>
        /// Returns contact corresponding to current user.
        /// </summary>
        static string currentContactKey = "CDM_CurrentContact";
        public static Logic.Entities.Contact CurrentContact
        {
            get
            {
                IDictionary items = HttpContext.Current.Items;
                if (!items.Contains(currentContactKey))
                {
                    try
                    {
                        string contactId =
                            (string)HttpContext.Current.Profile.GetPropertyValue("CDM_UserId");
                        items[currentContactKey] = CurrentEndpoint.GetContactById(contactId);
                    }
                    catch (SettingsPropertyNotFoundException)
                    {
                        // Just ignore it for now
                    }
                }
                return (Logic.Entities.Contact)items[currentContactKey];
            }
        }

        /// <summary>
        /// Returns contact corresponding to current user.
        /// </summary>
        static string currentCDMUserKey = "CDM_CurrentCDMUser";
        public static Logic.Entities.User CurrentCDMUser
        {
            get
            {
                IDictionary items = HttpContext.Current.Items;
                if (!items.Contains(currentCDMUserKey))
                {
                    try
                    {
                        string userId =
                            (string)HttpContext.Current.Profile.GetPropertyValue("CDM_UserId");
                        items[currentCDMUserKey] = CurrentEndpoint.GetCDMUserById(userId);
                    }
                    catch (SettingsPropertyNotFoundException)
                    {
                        // Just ignore it for now
                    }
                }
                return (Logic.Entities.User)items[currentCDMUserKey];
            }
        }

        /// <summary>
        /// Returns company corresponding to current user.
        /// </summary>
        static string currentCompanyKey = "CDM_CurrentContact_Company";
        public static Logic.Entities.Company CurrentCompany
        {
            get
            {
                IDictionary items = HttpContext.Current.Items;
                if (!items.Contains(currentCompanyKey))
                {
                    Logic.Entities.Contact contact = CurrentContact;
                    if (contact != null)
                    {
                        items[currentCompanyKey] =
                            CurrentEndpoint.GetCompanyByContactId(contact.Id);
                    }
                }
                return (Logic.Entities.Company)items[currentCompanyKey];
            }
        }

        /// <summary>
        /// Check if current contact has access to given activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool HasAccess(Logic.Entities.Activity activity)
        {
            if (activity.DeliveryPackageId != null)
            {
                Logic.Entities.DeliveryPackage package =
                    CurrentEndpoint.GetDeliveryPackageById(activity.DeliveryPackageId,
                        CurrentCompany);
                return package != null;
            }
            return false;
        }

        /// <summary>
        /// Check if current contact has access to given contact.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static bool HasAccess(Logic.Entities.Contact contact)
        {
            Logic.Entities.Company company = CurrentEndpoint.GetCompanyByContactId(contact.Id);
            if (company != null && company.Id == CurrentCompany.Id)
            {
                return true;
            }
            return false;
        }
    }
}
