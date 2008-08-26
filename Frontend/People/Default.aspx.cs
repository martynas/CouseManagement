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

        protected void rptrContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.Contact contact = (Logic.Entities.Contact)e.Item.DataItem;

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
            }
        }

        protected void rptrContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                List<Logic.Entities.Contact> contacts;
                contacts = new List<Logic.Entities.Contact>();
                foreach (RepeaterItem item in rptrContacts.Items)
                {
                    HyperLink hlEmail = (HyperLink)item.FindControl("hlEmail");
                    CheckBox cbSelected = (CheckBox)item.FindControl("cbSelected");
                    if (hlEmail != null && cbSelected != null && cbSelected.Checked)
                    {
                        Logic.Entities.Contact contact;
                        contact = Endpoint.GetContactByEmail(hlEmail.Text, CurrentCompany);
                        // Cannot delete current user
                        if (contact != null && contact.Id != CurrentContact.Id)
                        {
                            contacts.Add(contact);
                        }
                    }
                }
                Endpoint.DeleteContacts(contacts);
                LoadContacts();
            }
        }
    }
}
