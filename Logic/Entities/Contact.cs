using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic.Entities
{
    /// <summary>
    /// Contact entity.
    /// </summary>
    [Serializable]
    public class Contact : AbstractEntity
    {
        private string title;
        private string firstName;
        private string lastName;
        private string email;
        private string directPhone;
        private string mobilePhone;
        private string companyID;

        public Contact()
        {
        }

        public Contact(string id)
        {
            this.Id = id;
        }

        #region Properties
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string DirectPhone
        {
            get { return directPhone; }
            set { directPhone = value; }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        public string PrimaryCompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        #endregion

        /// <summary>
        /// Convert contact to string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }

    /// <summary>
    /// Contact comparer.
    /// </summary>
    public class ContactLexicographicalComparer : IComparer<Contact>
    {
        #region IComparer<Contact> Members
        public int Compare(Contact x, Contact y)
        {
            if (x.LastName.CompareTo(y.LastName) == 0)
            {
                return x.FirstName.CompareTo(y.FirstName);
            }
            else
            {
                return x.LastName.CompareTo(y.LastName);
            }
        }
        #endregion
    }
}
