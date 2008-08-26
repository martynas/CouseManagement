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

namespace Atrendia.CourseManagement.Frontend
{
    public partial class AccountManager : Support.BaseUserControl
    {
        /// <summary>
        /// Delivery package ID. If not assigned, company account
        /// manager is shown; otherwise, user that is responsible
        /// for delivery package.
        /// </summary>
        public string DeliveryPackageId
        {
            get { return (string)ViewState["DeliveryPackageId"]; }
            set { ViewState["DeliveryPackageId"] = value; }
        }

        private Logic.Entities.User responsibleUser;
        /// <summary>
        /// Get the user that can be contacted. Depends on context,
        /// e.g. DeliveryPackageId and whether Helpers.CurrentCompany
        /// is available.
        /// </summary>
        public Logic.Entities.User ResponsibleUser
        {
            protected get
            {
                if (responsibleUser == null && DeliveryPackageId != null)
                {
                    responsibleUser = Endpoint.GetAccountManagerByDeliveryPackageId(DeliveryPackageId);
                }
                if (responsibleUser == null && CurrentCompany != null)
                {
                    responsibleUser = Endpoint.GetAccountManagerByCompanyId(CurrentCompany.Id);
                }
                return responsibleUser;
            }
            set
            {
                responsibleUser = value;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (ResponsibleUser != null)
            {
                lblTitle.Text = ResponsibleUser.ToString();
                if (!string.IsNullOrEmpty(ResponsibleUser.DirectPhone))
                {
                    pDirectPhone.Visible = true;
                    lblDirectPhone.Text = ResponsibleUser.DirectPhone;
                }
                if (!string.IsNullOrEmpty(ResponsibleUser.MobilePhone))
                {
                    pMobilePhone.Visible = true;
                    lblMobilePhone.Text = ResponsibleUser.MobilePhone;
                }
                if (!string.IsNullOrEmpty(ResponsibleUser.Email))
                {
                    pEmail.Visible = true;
                    hlEmail.Text = ResponsibleUser.Email;
                    hlEmail.NavigateUrl = string.Format("mailto:{0}", ResponsibleUser.Email);
                }
            }
            else
            {
                Visible = false;
            }
        }
    }
}