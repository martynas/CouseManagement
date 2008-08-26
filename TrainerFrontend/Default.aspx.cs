using System;

namespace Atrendia.CourseManagement.TrainerFrontend
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Calendar/Default.aspx");
        }
    }
}
