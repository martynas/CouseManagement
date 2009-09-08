using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Diagnostics;

namespace Atrendia.CourseManagement.Frontend.Support.Controls
{
    public partial class PlannedCourses : BaseUserControl
    {

        #region Contants
        const String AttrContactID = "AttrContactID";
        const String AttrProductGroupID = "AttrProductGroupID";
        #endregion

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
            rptTable.DataSource = Contacts;
            rptTable.DataBind();
        }

        private IList<Contact2ProductGroup> GetContactPGInfo(Logic.Entities.Contact contact)
        {
            // TODO: this method should check which Courses the Contact has already selected and so on
            IList<Contact2ProductGroup> pgInfo = new List<Contact2ProductGroup>();
            foreach (Logic.Entities.ProductGroup pg in AllProducts)
            {
                pgInfo.Add(new Contact2ProductGroup()
                {
                    Contact = contact,
                    ProductGroup = pg,
                    Selected = false
                });
            }

            return pgInfo;
        }

        public bool ValidatePGInfo(out IList<Logic.VirtualEntities.Contact2ProductGroups> pgInfo)
        {
            Dictionary<string, Logic.Entities.ProductGroup> productMap = 
                new Dictionary<string, Logic.Entities.ProductGroup>();
            foreach (Logic.Entities.ProductGroup pg in AllProducts)
            {
                productMap.Add(pg.Id, pg);
            }

            pgInfo = new List<Logic.VirtualEntities.Contact2ProductGroups>();
            foreach (RepeaterItem row in rptTable.Items)
            {
                HiddenField hfContactID = (HiddenField)row.FindControl("hfContactID");
                Repeater rptTableItem = (Repeater)row.FindControl("rptTableItem");

                Logic.VirtualEntities.Contact2ProductGroups info = new
                    Logic.VirtualEntities.Contact2ProductGroups(Endpoint.GetContactById(hfContactID.Value));

                foreach (RepeaterItem item in rptTableItem.Items)
                {
                    CheckBox cbProduct = (CheckBox)item.FindControl("cbProduct");

                    if (cbProduct.Checked)
                    {
                        info.ProductGroups.Add(productMap[cbProduct.Attributes[AttrProductGroupID]]);
                    }
                }
                pgInfo.Add(info);
            }
            return true;
        }

        #region Data binding
        protected void rptrTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                // Binding Header of the list
                Repeater rptTableHeader = (Repeater)e.Item.FindControl("rptTableHeader");
                rptTableHeader.DataSource = AllProducts;
                rptTableHeader.DataBind();
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Logic.Entities.Contact contact = (Logic.Entities.Contact)e.Item.DataItem;

                Literal ltContact = (Literal)e.Item.FindControl("ltContact");
                Repeater rptTableItem = (Repeater)e.Item.FindControl("rptTableItem");
                HiddenField hfContactID = (HiddenField)e.Item.FindControl("hfContactID");

                ltContact.Text = contact.ToString();
                Debug.Assert(!String.IsNullOrEmpty(contact.Id), "contact.Id not set");
                hfContactID.Value = contact.Id;

                rptTableItem.DataSource = GetContactPGInfo(contact);
                rptTableItem.DataBind();
            }
        }

        // Binding Header, listing all available Product Groups
        protected void rptrTableHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.ProductGroup pg = (Logic.Entities.ProductGroup)e.Item.DataItem;

                Literal ltProductTitle = (Literal)e.Item.FindControl("ltProductTitle");
                CheckBox cbProductTitle = (CheckBox)e.Item.FindControl("cbProductTitle");

                ltProductTitle.Text = pg.Heading;
            }
        }

        // Binding an Item
        protected void rptrTableItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Contact2ProductGroup pg = (Contact2ProductGroup)e.Item.DataItem;

                CheckBox cbProduct = (CheckBox)e.Item.FindControl("cbProduct");

                cbProduct.Checked = pg.Selected;
                cbProduct.Attributes.Add(AttrContactID, pg.Contact.Id);
                cbProduct.Attributes.Add(AttrProductGroupID, pg.ProductGroup.Id);
            }
        }
        #endregion

    }

    class Contact2ProductGroup
    {

        public Logic.Entities.Contact Contact;
        public Logic.Entities.ProductGroup ProductGroup;
        public Boolean Selected;

    }
}