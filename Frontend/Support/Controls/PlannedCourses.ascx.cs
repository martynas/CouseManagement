using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Atrendia.CourseManagement.Frontend.Support.Controls
{
    public partial class PlannedCourses : BaseUserControl
    {

        #region Private variables
        private IList<Logic.Entities.Contact> contacts;
        private IList<Logic.Entities.ProductGroup> allproducts;
        private IList<Logic.Entities.ProductGroup> AllProducts
        {
            get
            {
                if (allproducts == null)
                {
                    allproducts = Endpoint.GetAllProductGroups();
                }
                return allproducts;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IList<Logic.Entities.Contact> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                UpdateControl();
            }
        }

        public void UpdateControl()
        {
        }

        #region Data binding
        protected void rptrTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                // Binding Header of the list
                Repeater rptTableHeader = (Repeater)e.Item.FindControl("rptTableHeader");
                rptTableHeader.DataSource = AllProducts;
                rptTableHeader.DataBinding();
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Logic.Entities.Contact contact = (Logic.Entities.Contact)e.Item.DataItem;

                Literal ltContact = (Literal)e.Item.FindControl("ltContact");
                Repeater rptTableItem = (Repeater)e.Item.FindControl("rptTableItem");

                ltContact = contact.Title;

                rptTableHeader.DataSource = AllProducts;
                rptTableHeader.DataBinding();
            }
        }

        // Binding Header, listing all available Product Groups
        protected void rptrTableHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            { 
            }
        }

        // Binding an Item
        protected void rptrTableItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
            }
        }

        #endregion

    }
}