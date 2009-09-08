using System;
using System.Collections.Generic;

namespace Atrendia.CourseManagement.Logic.VirtualEntities
{
    public class Contact2ProductGroups
    {

        private Entities.Contact contact;
        private IList<Entities.ProductGroup> productGroups;

        public Entities.Contact Contact
        {
            get { return contact; }
        }

        public IList<Entities.ProductGroup> ProductGroups
        {
            get { return productGroups; }
        }

        public Contact2ProductGroups(Entities.Contact contact)
        {
            this.contact = contact;
            productGroups = new List<Entities.ProductGroup>();
        }

    }
}
