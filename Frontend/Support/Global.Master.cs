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

namespace Atrendia.CourseManagement.Frontend.Support
{
    public partial class GlobalMaster : System.Web.UI.MasterPage
    {
        protected String ContactName
        {
            get
            {
                Logic.Entities.Contact contact = Atrendia.CourseManagement.Helpers.Endpoint.CurrentContact;
                if (contact != null)
                {
                    return contact.ToString();
                }
                else
                {
                    Response.Redirect("~/Logout.aspx");
                }
                return string.Empty;
            }
        }

        protected string CompanyName
        {
            get
            {
                Logic.Entities.Company company = Atrendia.CourseManagement.Helpers.Endpoint.CurrentCompany;
                if (company != null)
                {
                    return company.ToString();
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
