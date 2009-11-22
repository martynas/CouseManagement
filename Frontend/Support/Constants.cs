using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Atrendia.CourseManagement.Frontend.Support.Constants
{

    public static class Session {
        public const string PeopleUploadInfo = "PeopleUploadInfo";
    }

    public static class Pages
    {
        public const string PeopleUpload = "~/People/Upload.aspx";
        public const string PeopleEdit = "~/People/Edit.aspx?Id={0}";
        public const string PeopleEditWithReturn = "~/People/Edit.aspx?contact={0}&return={1}";
    }

    public static class PeopleParams
    {
        public const string Contact = "Id";
        public const string Return = "return";
    }

}
