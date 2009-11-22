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
        public Contact EditedContact
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
            }
        }
    }
}
