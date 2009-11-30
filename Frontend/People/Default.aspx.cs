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

namespace Atrendia.CourseManagement.Frontend.People
{
    public partial class Default : Support.BasePage
    {

        protected const string AttrContactID = "CDMContactID";

        /// <summary>
        /// Current page number.
        /// </summary>
        private int PageNumber
        {
            get
            {
                return ViewState["PageNumber"] != null ? (int)ViewState["PageNumber"] : 1;
            }
            set
            {
                ViewState["PageNumber"] = value;
            }
        }

        private Logic.Entities.Company CurrentCompany
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentCompany; }
        }

        private Logic.Entities.Contact CurrentContact
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentContact; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadContacts();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["PeopleUploadInfo"] != null)
            {
                pInfoUpload.Visible = true;
                pHelpUpload.Visible = false;
                lblInfoUpload.Text = (string)Session["PeopleUploadInfo"];
                Session.Remove("PeopleUploadInfo");
            }
            else
            {
                pInfoUpload.Visible = false;
                pHelpUpload.Visible = true;
            }
        }

        #region Event Handling
        protected void lbContacts_Click(object sender, EventArgs e)
        {
            mvContacts.SetActiveView(viewDetails);
            liCourses.Attributes.Add( "class", "" );
            liContacts.Attributes.Add("class", "active");
        }

        protected void lbCourses_Click(object sender, EventArgs e)
        {
            mvContacts.SetActiveView(viewCourses);
            liCourses.Attributes.Add("class", "active");
            liContacts.Attributes.Add("class", "");
            LoadCoursePanning();
        }
        #endregion

        #region Contacts details
        private void LoadContacts()
        {
            List<Logic.Entities.Contact> contacts;
            contacts = Endpoint.GetContactsByCompanyId(CurrentCompany.Id);
            if (contacts.Count > 0)
            {
                rptrContacts.DataSource = contacts;
                rptrContacts.DataBind();
            }
            else
                Response.Redirect(Support.Constants.Pages.PeopleUpload);
        }

        protected void rptrContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.Contact contact = (Logic.Entities.Contact)e.Item.DataItem;

                Label lbTitle = (Label)e.Item.FindControl("lbTitle");
                HyperLink hlName = (HyperLink)e.Item.FindControl("hlName");
                HyperLink hlEmail = (HyperLink)e.Item.FindControl("hlEmail");
                Label lbPhone = (Label)e.Item.FindControl("lbPhone");
                HyperLink hlView = (HyperLink)e.Item.FindControl("hlView");
                HyperLink hlEdit = (HyperLink)e.Item.FindControl("hlEdit");
                CheckBox cbSelected = (CheckBox)e.Item.FindControl("cbSelected");

                cbSelected.Attributes.Add(AttrContactID, contact.Id);
                string encodedContactID = Helpers.General.Encode(contact.Id);

                // Details
                lbTitle.Text = contact.Title;
                hlName.Text = contact.ToString();
                hlName.NavigateUrl = string.Format(Support.Constants.Pages.PeopleView,
                    encodedContactID);
                hlEmail.Text = contact.Email;
                hlEmail.NavigateUrl = string.Format("mailto:{0}", contact.Email);
                lbPhone.Text = contact.MobilePhone;

                // Actions
                hlView.NavigateUrl = string.Format(Support.Constants.Pages.PeopleView,
                    encodedContactID);
                hlEdit.NavigateUrl = string.Format(Support.Constants.Pages.PeopleEdit,
                    encodedContactID);
            }
        }

        protected void rptrContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<Logic.Entities.Contact> contacts;
            contacts = new List<Logic.Entities.Contact>();
            foreach (RepeaterItem item in rptrContacts.Items)
            {
                CheckBox cbSelected = (CheckBox)item.FindControl("cbSelected");
                if (cbSelected != null && cbSelected.Checked)
                {
                    Logic.Entities.Contact contact;
                    string contactID = cbSelected.Attributes[AttrContactID];

                    contact = Endpoint.GetContactById(contactID);
                    // Cannot delete current user
                    if (contact.Id != CurrentContact.Id && Endpoint.HasAccess(CurrentContact, contact))
                        contacts.Add(contact);
                }
            }

            if (e.CommandName == "Delete")
            {
                Endpoint.DeleteContacts(contacts);
                LoadContacts();
            }
        }
        #endregion

        #region Course Planning
        private void LoadCoursePanning()
        {
            cpCoursePlanning.Contacts = Endpoint.GetContactsByCompanyId(CurrentCompany.Id);
        }
        #endregion
    }
}
