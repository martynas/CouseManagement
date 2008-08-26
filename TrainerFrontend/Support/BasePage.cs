using System;

namespace Atrendia.CourseManagement.TrainerFrontend.Support
{
    public class BasePage : System.Web.UI.Page
    {
        protected Logic.IEndpoint Endpoint
        {
            get { return Helpers.Endpoint.CurrentEndpoint; }
        }

        protected Logic.Entities.User CurrentCDMUser
        {
            get { return Helpers.Endpoint.CurrentCDMUser; }
        }

        protected bool ValidateSession()
        {
            if (CurrentCDMUser == null)
            {
                Response.Redirect("~/Login.aspx");
                return false;
            } else
                return true;
        }

    }
}
