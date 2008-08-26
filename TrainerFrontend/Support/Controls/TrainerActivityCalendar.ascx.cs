using System;
using System.Collections.Generic;

namespace Atrendia.CourseManagement.TrainerFrontend.Support.Controls
{
    public partial class TrainerActivityCalendar : System.Web.UI.UserControl
    {
        private Logic.IEndpoint Endpoint
        {
            get { return Helpers.Endpoint.CurrentEndpoint; }
        }

        private Logic.Entities.User CurrentCDMUser
        {
            get { return Helpers.Endpoint.CurrentCDMUser; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && CurrentCDMUser != null)
            {
                DateTime today;
                try
                {
                    today = DateTime.ParseExact(Request["start"], "yyyy-MM-dd", null);
                }
                catch
                {
                    today = DateTime.Now.Date;
                }
                DateTime firstDay = today.AddDays(-today.Day + 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                hlPrevious.NavigateUrl = ResolveUrl(string.Format("~/Calendar/?mode=month&start={0}",
                    firstDay.AddMonths(-1).ToString("yyyy-MM-dd")));
                hlNext.NavigateUrl = ResolveUrl(string.Format("~/Calendar/?mode=month&start={0}",
                    firstDay.AddMonths(1).ToString("yyyy-MM-dd")));
                
                hlMonth.Text = today.ToString("MMMM yyyy");
                List<Logic.Entities.Activity> activities =
                    Endpoint.GetActivitiesByUserId(CurrentCDMUser.Id, null, firstDay, lastDay, false,
                    Logic.Entities.ActivityTypeFlag.Course);

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