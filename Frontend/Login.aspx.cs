using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Atrendia.CourseManagement.Frontend
{
    public partial class Login : Support.BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentContact != null)
            {
                if (HttpContext.Current.User.IsInRole(Helpers.UserRole.ClientPM))
                {
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    Session.Clear();
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

    }
}
