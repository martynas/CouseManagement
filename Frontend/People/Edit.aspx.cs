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
using Atrendia.CourseManagement.Logic.Entities;

namespace Atrendia.CourseManagement.Frontend.People
{
    public partial class Edit : Support.BasePage
    {

        private Contact editedContact;

        /// <summary>
        /// Returns instance of the Participant which is currently being edited.
        /// If the contact doesn't exists, then returns newly created object of type Contact.
        /// </summary>
        /// <returns></returns>
        protected Contact EditedContact
        {
            get
            {
                if (editedContact == null)
                {
                    string contactID = Helpers.General.
                        Decode(Request.Params[Support.Constants.PeopleParams.Contact]);
                    editedContact = Endpoint.GetContactById(contactID);

                    if (editedContact == null || !Endpoint.HasAccess(CurrentContact, editedContact))
                    // We are either creating a new contact 
                    // or the user has no rights to edit the requested contact.
                        editedContact = new Contact(); 
                }
                return editedContact;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (String.IsNullOrEmpty(EditedContact.Id))
                    // New Participant input mode
                    mvHeader.SetActiveView(vCreate);
                else
                    // Edit mode
                    mvHeader.SetActiveView(vEdit);

                BindEntityToView(EditedContact);
            }
        }

        protected void NavigateBack()
        {
            string returnUrl = Request.Params[Support.Constants.PeopleParams.ReturnUrl];
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Support.Constants.Pages.PeopleDefault;
            Response.Redirect(returnUrl);
        }

        #region Event
        protected void btnSave_click(Object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                LoadEntityFromView(EditedContact);
                Endpoint.SaveOrUpdateContact(EditedContact);
                NavigateBack();
            }
        }

        protected void btnCancel_click(Object sender, EventArgs e)
        {
            NavigateBack();
        }
        #endregion

        #region Binding
        protected void BindEntityToView(Contact contact)
        {
            tbTitle.Text = contact.Title;
            tbFirstName.Text = contact.FirstName;
            tbLastName.Text = contact.LastName;
            tbEmail.Text = contact.Email;
            tbMobilePhone.Text = contact.MobilePhone;
        }

        protected void LoadEntityFromView(Contact contact)
        {
            contact.Title = tbTitle.Text;
            contact.FirstName = tbFirstName.Text;
            contact.LastName = tbLastName.Text;
            contact.Email = tbEmail.Text;
            contact.MobilePhone = tbMobilePhone.Text;
        }
        #endregion
    }
}
