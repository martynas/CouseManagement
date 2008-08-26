using System;

namespace Atrendia.CourseManagement.TrainerFrontend.Support
{
    public class BaseUserControl : System.Web.UI.UserControl
    {

        protected Logic.IEndpoint Endpoint
        {
            get { return Helpers.Endpoint.CurrentEndpoint; }
        }

    }
}
