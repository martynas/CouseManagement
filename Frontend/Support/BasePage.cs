using System;

namespace Atrendia.CourseManagement.Frontend.Support
{
    public class BasePage : System.Web.UI.Page
    {

        #region properties
        protected string ActivityID
        {
            get
            {
                string s = Request["activity"];
                return s != null ? Atrendia.CourseManagement.Helpers.General.Decode(s) : string.Empty;
            }
        }

        protected Logic.IEndpoint Endpoint
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentEndpoint; }
        }

        protected Logic.Entities.Company CurrentCompany
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentCompany; }
        }

        protected Logic.Entities.Contact CurrentContact
        {
            get { return Atrendia.CourseManagement.Helpers.Endpoint.CurrentContact; }
        }

        private Logic.Entities.Activity _activity;
        protected Logic.Entities.Activity Activity
        {
            get
            {
                if (_activity == null)
                {
                    _activity = Endpoint.GetActivityById(ActivityID);
                }
                return _activity;
            }
        }
        #endregion

    }
}
