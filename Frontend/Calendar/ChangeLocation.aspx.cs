using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Atrendia.CourseManagement.Frontend.Calendar
{
    public partial class ChangeLocation : Support.BasePageActivity
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Activity == null ||
                !Support.Helpers.HasAccess(Activity) ||
                Activity.TypeFlag != Logic.Entities.ActivityTypeFlag.Course ||
                Activity.IsReadOnly)
            {
                Response.Redirect("~/Calendar/");
            }
            if (!Page.IsPostBack)
            {
                tahActivityHeading.Activity = Activity;

                tbLocation.Text = Activity.TrainingLocation;
                amResponsibleUser.ResponsibleUser = Trainer;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Endpoint.UpdateTrainingLocation(Activity, tbLocation.Text);
            RedirectBack();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(string.Format("~/Calendar/View.aspx?activity={0}",
                    Helpers.General.Encode(ActivityID)));
        }
    }
}
