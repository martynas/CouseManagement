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

namespace Atrendia.CourseManagement.Frontend.Overview
{
    public partial class Default : Support.BasePage
    {
        private int showCount = 5;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve all activities in near future
            List<Logic.Entities.Activity> activities;
            activities = Endpoint.GetActivities(CurrentContact.Id,
                showCount * 20, DateTime.Now.AddDays(-5), null);

            // Preparations
            List<Logic.Entities.Activity> preparation = activities.FindAll(
                delegate(Logic.Entities.Activity activity)
                {
                    return activity.TypeFlag == Logic.Entities.ActivityTypeFlag.Preparation;
                });
            if (preparation.Count > 0)
            {
                rptrPreparation.DataSource = preparation.GetRange(0, Math.Min(showCount, preparation.Count));
                rptrPreparation.DataBind();
            }
            else
            {
                pnlPreparationNone.Visible = true;
            }

            // Courses
            List<Logic.Entities.Activity> courses = activities.FindAll(
                delegate(Logic.Entities.Activity activity)
                {
                    return activity.TypeFlag == Logic.Entities.ActivityTypeFlag.Course;
                });
            if (courses.Count > 0)
            {
                rptrCourses.DataSource = courses.GetRange(0, Math.Min(showCount, courses.Count));
                rptrCourses.DataBind();
            }
            else
            {
                pnlCourseNone.Visible = true;
            }
        }

        protected void rptrPreparation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.Activity activity = (Logic.Entities.Activity)e.Item.DataItem;
                Label lblDate = (Label)e.Item.FindControl("lblDate");
                Label lblDone = (Label)e.Item.FindControl("lblDone");
                HyperLink hlActivity = (HyperLink)e.Item.FindControl("hlActivity");
                lblDate.Text = activity.Date.ToString("d MMM");
                lblDone.Visible = activity.IsDone;
                hlActivity.NavigateUrl = ResolveUrl(String.Format(
                    "~/Calendar/View.aspx?activity={0}",
                    Helpers.General.Encode(activity.Id)));
            }
        }

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
                    lblTime.Text = activity.TimeToString();
                }
                catch
                {
                    lblTime.Visible = false;
                }
            }
        }    
    }
}
