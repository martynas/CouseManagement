using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Atrendia.CourseManagement.Frontend.Support.Controls
{
    public partial class ActivityCalendar : Support.BaseUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime today = DateTime.Now;
                DateTime firstDay = today.AddDays(-today.Day + 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                
                hlMonth.Text = DateTime.Now.ToString("MMMM yyyy");
                List<Logic.Entities.Activity> activities =
                    Endpoint.GetActivities(CurrentContact.Id, null, firstDay, lastDay);

                foreach (Logic.Entities.Activity activity in activities)
                {
                    // Only showing trainings and preparation activities
                    if (activity.TypeFlag != Logic.Entities.ActivityTypeFlag.None)
                    {
                        miniCalendar.DayLinks[activity.Date.Day] = ResolveUrl(
                            string.Format("~/Calendar/?mode=day&start={0}",
                                activity.Date.ToString("yyyy-MM-dd")));
                    }
                }
            }
        }
    }
}