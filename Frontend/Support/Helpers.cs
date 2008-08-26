///
/// Helpers for the frontend application
///

using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace Atrendia.CourseManagement.Frontend.Support
{
    class Helpers
    {

        private static Logic.IEndpoint CurrentEndpoint
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentEndpoint; }
        }

        private static Logic.Entities.Company CurrentCompany
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentCompany; }
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
                Logic.Entities.DeliveryPackage package = CurrentEndpoint.
                        GetDeliveryPackageById(activity.DeliveryPackageId, CurrentCompany);
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
