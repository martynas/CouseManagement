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

namespace Atrendia.CourseManagement.Frontend.Calendar
{
    public partial class View : Support.BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Logic.Entities.Activity activity = Endpoint.GetActivityById(ActivityID);
            if (activity == null || 
                !Support.Helpers.HasAccess(activity) ||
                activity.TypeFlag == Logic.Entities.ActivityTypeFlag.None)
            {
                Response.Redirect("~/Calendar/");
            }
            else if (activity.TypeFlag == Logic.Entities.ActivityTypeFlag.Course)
            {
                Response.Redirect(string.Format("~/Calendar/Course.aspx?activity={0}",
                    Helpers.General.Encode(activity.Id)));
            }
            else
            {
                Response.Redirect(string.Format("~/Calendar/Preparation.aspx?activity={0}",
                    Helpers.General.Encode(activity.Id)));
            }
        }
    }
}
