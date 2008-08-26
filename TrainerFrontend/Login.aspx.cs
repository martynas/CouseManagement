using System;

namespace Atrendia.CourseManagement.TrainerFrontend
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atrendia.CourseManagement.Helpers.Endpoint.CurrentCDMUser != null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}
