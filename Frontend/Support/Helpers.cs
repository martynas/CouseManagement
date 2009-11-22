///
/// Helpers for the frontend application
///

using System.Collections;
using System.Configuration;
using System.Web.Configuration;
using Atrendia.CourseManagement.Helpers;
using Atrendia.CourseManagement.Logic.Entities;

namespace Atrendia.CourseManagement.Frontend.Support
{
    class Helpers
    {

        private static Logic.IEndpoint CurrentEndpoint
        {
            get { return Endpoint.CurrentEndpoint; }
        }

        private static Company CurrentCompany
        {
            get { return Endpoint.CurrentCompany; }
        }
        /// <summary>
        /// Check if current contact has access to given activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool HasAccess(Activity activity)
        {
            if (activity.DeliveryPackageId != null)
            {
                DeliveryPackage package = CurrentEndpoint.
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
        public static bool HasAccess(Contact contact)
        {
            return CurrentEndpoint.HasAccess(Endpoint.CurrentContact, contact);
        }
    }
}
