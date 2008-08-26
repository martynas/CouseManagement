using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Atrendia.CourseManagement.TrainerFrontend.Overview
{
    public partial class Default : Support.BasePage
    {
        private int showCount = 5;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ValidateSession())
                return;

            DateTime today;
            try
            {
                today = DateTime.ParseExact(Request["start"], "yyyy-MM-dd", null);
            }
            catch
            {
                today = DateTime.Now.Date;
            }

            // Retrieve all activities in near future
            List<Logic.Entities.Activity> activities;
            activities = Endpoint.GetActivitiesByUserId(CurrentCDMUser.Id,
                showCount * 20, today.AddDays(-5), null, false,
                Logic.Entities.ActivityTypeFlag.Course);

            List<Logic.Entities.Activity> courses = activities; // Later some filters might me applied here

            if (courses.Count > 0)
            {
                rptrCourses.DataSource = courses.GetRange(0, Math.Min(showCount, courses.Count));
                rptrCourses.DataBind();
                mvCoursePane.SetActiveView(vCourses);
            }
            else
            {
                mvCoursePane.SetActiveView(vCourseNone);
            }
        }

        #region Binders
        protected void rptrCourses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.Activity activity = (Logic.Entities.Activity)e.Item.DataItem;
                Label lblDate = (Label)e.Item.FindControl("lblDate");
                Label lblTime = (Label)e.Item.FindControl("lblTime");
                HyperLink hlActivity = (HyperLink)e.Item.FindControl("hlActivity");
                hlActivity.NavigateUrl = ResolveUrl(String.Format(
                    "~/Calendar/View.aspx?activity={0}",
                    Helpers.General.Encode(activity.Id)));
                lblDate.Text = activity.Date.ToString("d MMM");
                try
                {
                    lblTime.Text = String.Format("{0:00}:{1:00}-{2:00}:{3:00}",
                        activity.StartTime.Hours, activity.StartTime.Minutes,
                        activity.EndTime.Hours, activity.EndTime.Minutes);
                }
                catch
                {
                    lblTime.Visible = false;
                }
            }
        }
        #endregion
    }
}
