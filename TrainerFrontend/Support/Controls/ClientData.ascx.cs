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

namespace Atrendia.CourseManagement.TrainerFrontend.Support.Controls
{
    public partial class ClientData : Support.BaseUserControl
    {

        private Logic.Entities.Activity activity;
        public Logic.Entities.Activity Activity
        {
            get { return activity; }
            set { activity = value; }
        }

        private Logic.Entities.Company company;
        public Logic.Entities.Company Company
        {
            get
            {
                if (company == null)
                {
                    company = Endpoint.GetCompanyById(Activity.PrimaryCompanyId);
                }
                return company;
            }
        }

        private Logic.Entities.Contact contact;
        public Logic.Entities.Contact Contact
        {
            get
            {
                if (contact == null)
                {
                    contact = Endpoint.GetContactById(Activity.PrimaryContactId);
                }
                return contact;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}