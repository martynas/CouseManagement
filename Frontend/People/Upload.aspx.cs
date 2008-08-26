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
using System.Text.RegularExpressions;

namespace Atrendia.CourseManagement.Frontend.People
{
    public partial class Upload : Support.BaseUserControl
    {
        enum ViewMode
        {
            Initial,
            Review
        };

        enum ParseResultMode
        {
            Success,
            Warnings,
            NoData
        };

        /// <summary>
        /// View mode -- import window or review results.
        /// </summary>
        private ViewMode Mode
        {
            get
            {
                return pnlInitial.Visible ? ViewMode.Initial : ViewMode.Review;
            }
            set
            {
                pnlInitial.Visible = value == ViewMode.Initial;
                pnlReview.Visible = !pnlInitial.Visible;
            }
        }

        /// <summary>
        /// Parse result mode.
        /// </summary>
        private ParseResultMode? ParseMode
        {
            set
            {
                pParseNoData.Visible = pParseSuccess.Visible = pParseWarnings.Visible = false;
                pnlWarnings.Visible = false;
                if (value.HasValue)
                {
                    switch (value.Value)
                    {
                        case ParseResultMode.Success:
                            pParseSuccess.Visible = true;
                            break;
                        case ParseResultMode.Warnings:
                            pnlWarnings.Visible = true;
                            pParseWarnings.Visible = true;
                            break;
                        case ParseResultMode.NoData:
                            pParseNoData.Visible = true;
                            break;
                    }
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fuContacts.HasFile)
            {
                Mode = ViewMode.Review;
                using (Helpers.ContactFileParser parser =
                       new Helpers.ContactFileParser(fuContacts.FileContent))
                {
                    List<Logic.Entities.Contact> contacts = parser.Parse();
                    if (contacts.Count > 0)
                    {
                        if (parser.Errors.Count > 0)
                        {
                            ParseMode = ParseResultMode.Warnings;
                        }
                        else
                        {
                            ParseMode = ParseResultMode.Success;
                        }
                        rptrContacts.DataSource = contacts;
                        rptrContacts.DataBind();
                    }
                    else
                    {
                        ParseMode = ParseResultMode.NoData;
                    }
                    if (parser.Errors.Count > 0)
                    {
                        rptrWarnings.DataSource = parser.Errors;
                        rptrWarnings.DataBind();
                    }
                }
            }
            else
            {
                Mode = ViewMode.Initial;
                pNoFile.Visible = true;
            }
        }

        private Regex patternPhone = new Regex(@"^[-+.() 0-9]*$");
        private Regex patternEmail = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$",
            RegexOptions.IgnoreCase);

        private bool ValidateContacts(out List<Logic.Entities.Contact> list)
        {
            bool allValid = true;
            list = new List<Logic.Entities.Contact>();
            foreach (RepeaterItem item in rptrContacts.Items)
            {
                TextBox tbTitle = (TextBox)item.FindControl("tbTitle");
                TextBox tbFirstName = (TextBox)item.FindControl("tbFirstName");
                TextBox tbLastName = (TextBox)item.FindControl("tbLastName");
                TextBox tbEmail = (TextBox)item.FindControl("tbEmail");
                TextBox tbPhone = (TextBox)item.FindControl("tbPhone");

                Logic.Entities.Contact contact = new Logic.Entities.Contact();
                contact.Title = tbTitle.Text.Trim();
                contact.FirstName = tbFirstName.Text.Trim();
                contact.LastName = tbLastName.Text.Trim();
                contact.Email = tbEmail.Text.Trim();
                contact.MobilePhone = tbPhone.Text.Trim();

                // Validate
                List<string> validationMessages = new List<string>();
                if (contact.FirstName == string.Empty)
                {
                    validationMessages.Add("First name is required.");
                }
                if (contact.LastName == string.Empty)
                {
                    validationMessages.Add("Second name is required.");
                }
                if (contact.Email == string.Empty)
                {
                    validationMessages.Add("Email is required.");
                }
                else if (!patternEmail.Match(contact.Email).Success)
                {
                    validationMessages.Add("Email has invalid format.");
                }
                if (contact.MobilePhone != string.Empty &&
                    !patternPhone.Match(contact.MobilePhone).Success)
                {
                    validationMessages.Add("Phone must be composed of digits and other special characters.");
                }
                if (validationMessages.Count > 0)
                {
                    HtmlTableRow trMessages = (HtmlTableRow)item.FindControl("trMessages");
                    HtmlContainerControl pMessage = (HtmlContainerControl)item.FindControl("pMessage");
                    allValid = false;
                    trMessages.Visible = true;
                    pMessage.InnerHtml = string.Join("<br/>", validationMessages.ToArray());
                }
                else
                {
                    list.Add(contact);
                }
            }
            return allValid;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            ParseMode = null;
            List<Logic.Entities.Contact> contacts;
            int alreadyExist = 0;
            if (ValidateContacts(out contacts))
            {
                List<Logic.Entities.Contact> toAdd = new List<Logic.Entities.Contact>();
                foreach (Logic.Entities.Contact contact in contacts)
                {
                    Logic.Entities.Contact current = Endpoint.GetContactByEmail(contact.Email, CurrentCompany);
                    if (current != null)
                    {
                        ++alreadyExist;
                    }
                    else
                    {
                        toAdd.Add(contact);
                    }
                }
                if (toAdd.Count > 0)
                {
                    Endpoint.AddContactsToCompany(CurrentCompany, toAdd);
                }
                if (toAdd.Count + alreadyExist > 0)
                {
                    string message = string.Format("{0} new contacts were added.", toAdd.Count);
                    if (alreadyExist > 0)
                    {
                        message += string.Format(" In addition, {0} contacts " +
                            "were already in our database and were not modified.",
                            alreadyExist);
                    }
                    Session["PeopleUploadInfo"] = message; 
                }
                Response.Redirect("~/People/Default.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/People/Upload.aspx");
        }

        protected void rptrWarnings_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Helpers.ParsingError error = (Helpers.ParsingError)e.Item.DataItem;
                Label lblMessage = (Label)e.Item.FindControl("lblMessage");
                Label lblLineNumber = (Label)e.Item.FindControl("lblLineNumber");
                
                lblMessage.Text = error.Message;
                lblLineNumber.Text = error.LineNumber.ToString();
            }
        }

        protected void rptrContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.Contact contact = (Logic.Entities.Contact)e.Item.DataItem;
                TextBox tbTitle = (TextBox)e.Item.FindControl("tbTitle");
                TextBox tbFirstName = (TextBox)e.Item.FindControl("tbFirstName");
                TextBox tbLastName = (TextBox)e.Item.FindControl("tbLastName");
                TextBox tbEmail = (TextBox)e.Item.FindControl("tbEmail");
                TextBox tbPhone = (TextBox)e.Item.FindControl("tbPhone");
                
                tbTitle.Text = contact.Title;
                tbFirstName.Text = contact.FirstName;
                tbLastName.Text = contact.LastName;
                tbEmail.Text = contact.Email;
                tbPhone.Text = contact.MobilePhone;
            }
        }
    }
}
