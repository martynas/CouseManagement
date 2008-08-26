using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic.Entities
{
    [Serializable]
    public class User : AbstractEntity
    {
        private string title;
        private string firstName;
        private string lastName;
        private string directPhone;
        private string mobilePhone;
        private string email;

        public User()
        {
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
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        #endregion

        /// <summary>
        /// Convert User to string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Title, FirstName, LastName);
        }
    }
}
