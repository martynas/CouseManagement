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
    public partial class Preparation : Support.BasePageActivity
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Activity == null ||
                !Support.Helpers.HasAccess(Activity) ||
                Activity.TypeFlag != Logic.Entities.ActivityTypeFlag.Preparation)
            {
                Response.Redirect("~/Calendar/");
            }
            // Load
            lblHeading.Text = Activity.Title;
            lblDeadline.Text = Activity.Date.ToString("d MMM, yyyy");
            lblNotes.Text = new Helpers.RTF(Activity.Notes).ToText();
            amResponsibleUser.ResponsibleUser = Trainer;
            pnlNotDone.Visible = !Activity.IsDone;
        }

        protected void btnMarkAsDone_Click(object sender, EventArgs e)
        {
            Endpoint.MarkActivityAsDone(Activity);
            pnlNotDone.Visible = false;
        }
    }
}
