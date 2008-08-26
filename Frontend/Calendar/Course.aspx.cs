using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Atrendia.CourseManagement.Frontend.Calendar
{
    public partial class Course : Support.BasePageActivity
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Activity == null ||
                !Support.Helpers.HasAccess(Activity) ||
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

                amResponsibleUser.ResponsibleUser = Trainer;

                LoadParticipants();
                if (!Activity.IsReadOnly)
                    LoadContacts();
                else
                {
                    // Disabling functionality which is not relevant to ReadOnly (finished or past) Activities
                    hEmployees.Visible = false;
                }
            }
        }

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

        private void LoadContacts()
        {
            List<Logic.Entities.Contact> contacts;
            contacts = Endpoint.GetContactsByCompanyId(CurrentCompany.Id);
            if (contacts.Count > 0)
            {
                rptrContacts.DataSource = contacts;
                rptrContacts.DataBind();
            }
        }

        private List<Logic.Entities.Contact> GetRepeaterSelectedContacts(Repeater repeater)
        {
            List<Logic.Entities.Contact> contacts;
            contacts = new List<Logic.Entities.Contact>();
            foreach (RepeaterItem item in repeater.Items)
            {
                HiddenField hfContactCDMId = (HiddenField)item.FindControl("hfContactCDMId");
                CheckBox cbSelected = (CheckBox)item.FindControl("cbSelected");
                if (hfContactCDMId != null && cbSelected != null && cbSelected.Checked)
                {
                    Logic.Entities.Contact contact;
                    contact = Endpoint.GetContactById(hfContactCDMId.Value);
                    if (contact != null)
                    {
                        contacts.Add(contact);
                    }
                }
            }
            return contacts;
        }

        #region Participants

        protected void rptrParticipants_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblSeatsTaken = (Label)e.Item.FindControl("lblSeatsTaken");
                Label lblSeatsTotal = (Label)e.Item.FindControl("lblSeatsTotal");
                Button btnRemoveFromTraining = (Button)e.Item.FindControl("btnRemoveFromTraining");
                lblSeatsTaken.Text = Participants.Count.ToString();
                lblSeatsTotal.Text = DeliveryPackage.GroupSize.ToString();
                btnRemoveFromTraining.Visible = !Activity.IsReadOnly;
            }
            else if (e.Item.DataItem != null)
            {
                Logic.Entities.Contact contact = (Logic.Entities.Contact)e.Item.DataItem;

                CheckBox cbSelected = (CheckBox)e.Item.FindControl("cbSelected");
                HiddenField hfContactCDMId = (HiddenField)e.Item.FindControl("hfContactCDMId");
                Label lbTitle = (Label)e.Item.FindControl("lbTitle");
                HyperLink hlName = (HyperLink)e.Item.FindControl("hlName");
                HyperLink hlEmail = (HyperLink)e.Item.FindControl("hlEmail");
                Label lbPhone = (Label)e.Item.FindControl("lbPhone");

                hfContactCDMId.Value = contact.Id;
                lbTitle.Text = contact.Title;
                hlName.Text = contact.ToString();
                hlName.NavigateUrl = string.Format("~/People/View.aspx?Id={0}",
                    Helpers.General.Encode(contact.Id));
                hlEmail.Text = contact.Email;
                hlEmail.NavigateUrl = string.Format("mailto:{0}", contact.Email);
                lbPhone.Text = contact.MobilePhone;
                cbSelected.Visible = !Activity.IsReadOnly;
            }
        }

        protected void rptrParticipants_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                List<Logic.Entities.Contact> contacts;
                contacts = GetRepeaterSelectedContacts(rptrParticipants);
                Endpoint.RemoveParticipantsFromActivity(Activity, contacts);
                ReloadParticipants();
                LoadParticipants();
                LoadContacts();
            }
        }

        #endregion

        #region Employees


        protected void rptrContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.Contact contact = (Logic.Entities.Contact)e.Item.DataItem;

                HiddenField hfContactCDMId = (HiddenField)e.Item.FindControl("hfContactCDMId");
                CheckBox cbSelected = (CheckBox)e.Item.FindControl("cbSelected");
                Label lbTitle = (Label)e.Item.FindControl("lbTitle");
                HyperLink hlName = (HyperLink)e.Item.FindControl("hlName");
                HyperLink hlEmail = (HyperLink)e.Item.FindControl("hlEmail");
                Label lbPhone = (Label)e.Item.FindControl("lbPhone");

                foreach (Logic.Entities.Contact participant in Participants)
                {
                    if (participant.Id == contact.Id)
                    {
                        cbSelected.Visible = false;
                        break;
                    }
                }

                hfContactCDMId.Value = contact.Id;
                lbTitle.Text = contact.Title;
                hlName.Text = contact.ToString();
                hlName.NavigateUrl = string.Format("~/People/View.aspx?Id={0}",
                    Helpers.General.Encode(contact.Id));
                hlEmail.Text = contact.Email;
                hlEmail.NavigateUrl = string.Format("mailto:{0}", contact.Email);
                lbPhone.Text = contact.MobilePhone;
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Button btnAdd = (Button)e.Item.FindControl("btnAdd");
                btnAdd.Enabled = DeliveryPackage.GroupSize > Participants.Count;
            }
        }

        protected void rptrContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                List<Logic.Entities.Contact> contacts = GetRepeaterSelectedContacts(rptrContacts);
                int remaining = DeliveryPackage.GroupSize - Participants.Count;
                if (contacts.Count > remaining)
                {
                    // TODO: warn about limit
                    contacts = contacts.GetRange(0, remaining);
                }
                Endpoint.AddParticipantsToActivity(Activity, contacts);

                ReloadParticipants();
                LoadParticipants();
                LoadContacts();
            }
        }

        #endregion
    }
}
