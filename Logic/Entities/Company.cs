using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic.Entities
{
    public class Company : AbstractEntity
    {
        private string alias;

        public Company()
        {
        }

        #region Properties
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }
        #endregion

        public override string ToString()
        {
            return Alias;
        }
    }
}
