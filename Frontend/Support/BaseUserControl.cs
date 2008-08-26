using System;

namespace Atrendia.CourseManagement.Frontend.Support
{
    public class BaseUserControl : System.Web.UI.UserControl
    {

        protected Logic.IEndpoint Endpoint
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentEndpoint; }
        }

        protected Logic.Entities.Company CurrentCompany
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentCompany; }
        }

        protected Logic.Entities.Contact CurrentContact
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentContact; }
        }

    }
}
