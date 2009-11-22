using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic.COMEndpoint
{
    public partial class COMEndpoint
    {

        #region Security & Access Rights
        public bool HasAccess(Entities.Contact user, Entities.Contact obj)
        {
            return user.PrimaryCompanyID == obj.PrimaryCompanyID;
        }
        #endregion

    }
}
