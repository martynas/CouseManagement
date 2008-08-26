using System;
using Atrendia.CourseManagement.Logic.Entities;

namespace Atrendia.CourseManagement.Frontend.Support.Controls
{
    public partial class TrainingActivityHeading : BaseUserControl
    {
        #region Properties
        private Activity activity;
        public Activity Activity
        {
            set 
            { 
                activity = value;
                UpdateView();
            }
            get { return activity; }
        }

        private bool longVersion = true;
        public bool LongVersion
        {
            set { 
                longVersion = value;
                phLV1.Visible = phLV2.Visible = value;
            }
            get { return longVersion; }
        }

        private Logic.Entities.User trainer;
        protected Logic.Entities.User Trainer
        {
            get
            {
                if (trainer == null)
                {
                    trainer = Endpoint.GetTrainerForActivity(Activity);
                }
                return trainer;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                UpdateView();
        }

        protected void UpdateView() 
        {
            pnlMain.Visible = (Activity != null);
            if (Activity != null)
            {
                // Load
                lblHeading.Text = Activity.Title;
                lblTrainer.Text = Trainer.ToString();

                if (!Activity.IsReadOnly)
                {
                    lblLocation.Text = Activity.TrainingLocation;
                    if (String.IsNullOrEmpty(Activity.TrainingLocation))
                        nuChangeLocation.Text = "Please specify";
                }
                else
                {
                    nuChangeLocation.Visible = false;
                    pPastActivityWarning.Visible = true;
                }

                lblDate.Text = Activity.Date.ToString("d MMM");

                lblTime.Text = Activity.TimeToString();
                phTime.Visible = Activity.HasTime;

                nuChangeLocation.NavigateUrl = String.Format(
                    "~/Calendar/ChangeLocation.aspx?activity={0}", Request["activity"]);
                if (!String.IsNullOrEmpty(Activity.EmailPlatform))
                    lblEmailPlatform.Text = Activity.EmailPlatform;

                if (!String.IsNullOrEmpty(Activity.AddinVersion))
                    lblAddinVersion.Text = Activity.AddinVersion;
                if (!String.IsNullOrEmpty(Activity.Language))
                    lblLanguage.Text = Activity.Language;
            }
        }
    }
}