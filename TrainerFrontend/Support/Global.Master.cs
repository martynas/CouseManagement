using System;
using Atrendia.CourseManagement.Helpers;

namespace Atrendia.CourseManagement.Frontend.Support
{
    public partial class GlobalMaster : System.Web.UI.MasterPage
    {
        protected String UserName
        {
            get
            {
                if (Endpoint.CurrentCDMUser != null)
                {
                    return Endpoint.CurrentCDMUser.ToString();
                }
                else
                {
                    Response.Redirect("~/Logout.aspx");
                }
                return string.Empty;
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Prevent caching
            Response.Expires = 60;
            Response.AddHeader("pragma", "no-cache");
            Response.AddHeader("cache-control", "private");
            Response.CacheControl = "no-cache";
        }
    }
}
