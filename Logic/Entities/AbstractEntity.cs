using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic.Entities
{
    public abstract class AbstractEntity
    {
        private string id;

        #region Properties
        public string Id
        {
            get { return id; }
            internal set { id = value; }
        }
        #endregion

        #region Equals
        /// <summary>
        /// Overriden object.Equals()
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Company other = (Company)obj;
            if (this.Id != null && other.Id != null)
            {
                return this.Id == other.Id;
            }
            return false;
        }

        /// <summary>
        /// Overriden object.GetHashCode()
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return id != null ? id.GetHashCode() : 0;
        }
        #endregion

    }
}
