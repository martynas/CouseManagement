using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic.Entities
{
    public enum ActivityTypeFlag
    {
        None = 0,
        Course = 1,
        Preparation = 2
    }

    public class Activity : AbstractEntity
    {
        private string heading;
        private string notes;
        private bool isDone;
        private DateTime date;
        private TimeSpan? startTime;
        private TimeSpan? endTime;
        private ActivityTypeFlag typeFlag;
        private string deliveryPackageId;
        private string contentsHeading;
        private string contentsNotes;
        private string trainingLocation;
        private string emailPlatform;
        private string language;
        private string addinVersion;
        private string primaryContactId;
        private string primaryCompanyId;

        public Activity()
        {
        }

        #region Properties
        public string Title
        {
            get
            {
                if (typeFlag == ActivityTypeFlag.Course && 
                    !string.IsNullOrEmpty(ContentsHeading))
                {
                    return string.Format("{0} ({1})", Heading, ContentsHeading);
                }
                else
                {
                    return Heading;
                }
            }
        }

        public string Heading
        {
            get { return heading; }
            set { heading = value; }
        }

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public string ContentsHeading
        {
            get { return contentsHeading; }
            set { contentsHeading = value; }
        }

        public string ContentsNotes
        {
            get { return contentsNotes; }
            set { contentsNotes = value; }
        }

        public bool IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }

        public bool HasTime
        {
            get { return startTime.HasValue && endTime.HasValue; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public TimeSpan StartTime
        {
            get { return startTime.Value; }
            set { startTime = value; }
        }

        public TimeSpan EndTime
        {
            get { return endTime.Value; }
            set { endTime = value; }
        }

        public TimeSpan Duration
        {
            get { return EndTime - StartTime; }
        }

        public string DeliveryPackageId
        {
            get { return deliveryPackageId; }
            internal set { deliveryPackageId = value; }
        }

        public ActivityTypeFlag TypeFlag
        {
            get { return typeFlag; }
            internal set { typeFlag = value; }
        }

        public string TrainingLocation
        {
            get { return trainingLocation; }
            internal set { trainingLocation = value; }
        }

        public string EmailPlatform
        {
            get { return emailPlatform; }
            internal set { emailPlatform = value; }
        }

        public string Language
        {
            get { return language; }
            internal set { language = value; }
        }

        public string AddinVersion
        {
            get { return addinVersion; }
            internal set { addinVersion = value; }
        }

        public string PrimaryContactId
        {
            get { return primaryContactId; }
            internal set { primaryContactId = value; }
        }

        public string PrimaryCompanyId 
        {
            get { return primaryCompanyId; }
            internal set { primaryCompanyId = value; }
        }
        #endregion

        #region Helper functions
        public string TimeToString()
        {
            if (HasTime)
                return String.Format("{0:00}:{1:00}-{2:00}:{3:00}",
                        StartTime.Hours, StartTime.Minutes, EndTime.Hours, EndTime.Minutes);
            else
                return "";
        }

        public bool IsReadOnly
        {
            get { return (Date < DateTime.Today || IsDone); }
        }

        #endregion
    }
}
