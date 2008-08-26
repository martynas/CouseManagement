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
using System.Collections.Generic;

namespace Atrendia.CourseManagement.TrainerFrontend.Calendar
{
    public partial class Course : Support.BasePageActivity
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Activity == null ||
                Activity.TypeFlag != Logic.Entities.ActivityTypeFlag.Course)
            {
                Response.Redirect("~/Calendar/");
            }

            if (!IsPostBack)
            {
                tahActivityHeading.Activity = Activity;

                // Adding Notes and hiding empty components
                lblContentsNotes.Text = new Helpers.RTF(Activity.ContentsNotes).ToText();
                pContentsNotes.Visible = !String.IsNullOrEmpty(lblContentsNotes.Text);
                lblNotes.Text = new Helpers.RTF(Activity.Notes).ToText();
                pNotes.Visible = !String.IsNullOrEmpty(lblNotes.Text);
                hDescription.Visible = pNotes.Visible || pContentsNotes.Visible;

                LoadParticipants();
            }
        }

        #region Participants

        private void LoadParticipants()
        {
            pnlNoParticipants.Visible = Participants.Count == 0;
            if (Participants.Count > 0)
            {
                rptrParticipants.Visible = true;
                rptrParticipants.DataSource = Participants;
                rptrParticipants.DataBind();
            }
            else
            {
                rptrParticipants.Visible = false;
            }
        }

        protected void rptrParticipants_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblSeatsTaken = (Label)e.Item.FindControl("lblSeatsTaken");
                Label lblSeatsTotal = (Label)e.Item.FindControl("lblSeatsTotal");
                lblSeatsTaken.Text = Participants.Count.ToString();
                lblSeatsTotal.Text = DeliveryPackage.GroupSize.ToString();
            }
            else if (e.Item.DataItem != null)
            {
                Logic.Entities.Contact contact = (Logic.Entities.Contact)e.Item.DataItem;

                CheckBox cbSelected = (CheckBox)e.Item.FindControl("cbSelected");
                Label lbTitle = (Label)e.Item.FindControl("lbTitle");
                HyperLink hlName = (HyperLink)e.Item.FindControl("hlName");
                HyperLink hlEmail = (HyperLink)e.Item.FindControl("hlEmail");
                Label lbPhone = (Label)e.Item.FindControl("lbPhone");

                lbTitle.Text = contact.Title;
                hlName.Text = contact.ToString();
                hlName.NavigateUrl = string.Format("~/People/View.aspx?Id={0}",
                    Helpers.General.Encode(contact.Id));
                hlEmail.Text = contact.Email;
                hlEmail.NavigateUrl = string.Format("mailto:{0}", contact.Email);
                lbPhone.Text = contact.MobilePhone;
                cbSelected.Visible = !Activity.IsReadOnly;
                cbSelected.Checked = !ParticipatedContacts.Contains(contact.Id);
                cbSelected.Attributes.Add("ContactCDMId", contact.Id);
            }
        }

        protected void rptrParticipants_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //
        }

        protected void cbSelected_CheckedChanged(Object sender, EventArgs e)
        {
            CheckBox cbSelected = (sender as CheckBox);
            Logic.Entities.Contact contact = Endpoint.GetContactById(
                cbSelected.Attributes["ContactCDMId"]);
            Endpoint.SetParticipationStatus(Activity, contact, cbSelected.Checked);
            ReloadParticipatedContacts();
            LoadParticipants();
        }

        #endregion

        private List<Logic.Entities.Contact> GetRepeaterSelectedContacts(Repeater repeater)
        {
            List<Logic.Entities.Contact> contacts;
            contacts = new List<Logic.Entities.Contact>();
            foreach (RepeaterItem item in repeater.Items)
            {
                CheckBox cbSelected = (CheckBox)item.FindControl("cbSelected");
                if (cbSelected != null && cbSelected.Checked)
                {
                    Logic.Entities.Contact contact;
                    contact = Endpoint.GetContactById(cbSelected.Attributes["ContactCDMId"]);
                    if (contact != null)
                    {
                        contacts.Add(contact);
                    }
                }
            }
            return contacts;
        }
    }
}
