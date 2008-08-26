using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Atrendia.CourseManagement.Frontend.People
{
    public partial class View : Support.BasePage
    {   
        private Logic.Entities.Contact _contact;
        private Logic.Entities.Contact Contact
        {
            get
            {
                if (_contact == null)
                {
                    _contact = CurrentContact;
                    if (_contact == null || !Support.Helpers.HasAccess(_contact))
                    {
                        _contact = null;
                    }
                }
                return _contact;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Contact == null)
            {
                Response.Redirect("~/People/Default.aspx");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lblTitle.Text = Contact.ToString();
            hlEmail.Text = Contact.Email;
            hlEmail.NavigateUrl = string.Format("mailto:{0}", Contact.Email);
            if (!string.IsNullOrEmpty(Contact.MobilePhone))
            {
                phPhone.Visible = true;
                lblPhone.Text = Contact.MobilePhone;
            }

            List<Logic.Entities.Activity> activities = Endpoint.GetActivitiesForParticipant(Contact);
            if (activities.Count > 0)
            {
                pnlTrainings.Visible = true;
                pnlNoTrainings.Visible = false;
                rptrActivities.DataSource = activities;
                rptrActivities.DataBind();
            }
        }

        protected void rptrActivities_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.Activity activity = (Logic.Entities.Activity)e.Item.DataItem;
                Label lblDate = (Label)e.Item.FindControl("lblDate");
                HyperLink hlTitle = (HyperLink)e.Item.FindControl("hlTitle");
                Label lblTagCourse = (Label)e.Item.FindControl("lblTagCourse");
                Label lblTagPreparation = (Label)e.Item.FindControl("lblTagPreparation"); 

                lblDate.Text = activity.Date.ToString("dd MMM, yyyy");
                hlTitle.Text = activity.Title;
                hlTitle.NavigateUrl = string.Format("~/Calendar/View.aspx?activity={0}", 
                    Helpers.General.Encode(activity.Id));
                lblTagCourse.Visible = activity.TypeFlag == Logic.Entities.ActivityTypeFlag.Course;
                lblTagPreparation.Visible = activity.TypeFlag == Logic.Entities.ActivityTypeFlag.Preparation;
            }
        }
    }
}
