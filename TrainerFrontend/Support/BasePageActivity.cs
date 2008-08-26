using System;
using System.Collections.Generic;

namespace Atrendia.CourseManagement.TrainerFrontend.Support
{
    public class BasePageActivity : Support.BasePage
    {

        #region Properties
        protected string ActivityID
        {
            get
            {
                string s = Request["activity"];
                return s != null ? Helpers.General.Decode(s) : string.Empty;
            }
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

        private Logic.Entities.DeliveryPackage _delivaryPackage;
        protected Logic.Entities.DeliveryPackage DeliveryPackage
        {
            get
            {
                if (_delivaryPackage == null)
                {
                    _delivaryPackage = Endpoint.GetDeliveryPackageById(Activity.DeliveryPackageId, null);
                }
                return _delivaryPackage;
            }
        }

        private Logic.Entities.User _trainer;
        protected Logic.Entities.User Trainer
        {
            get
            {
                if (_trainer == null)
                {
                    _trainer = Endpoint.GetTrainerForActivity(Activity);
                }
                return _trainer;
            }
        }

        private List<Logic.Entities.Contact> _participants;
        protected List<Logic.Entities.Contact> Participants
        {
            get
            {
                if (_participants == null)
                {
                    _participants = Endpoint.GetParticipantsByActivityId(Activity.Id);
                }
                return _participants;
            }
        }
        protected void ReloadParticipants()
        {
            _participants = null;
            _participatedContacts = null;
        }

        private List<string> _participatedContacts;
        protected List<string> ParticipatedContacts
        {
            get
            {
                if (_participatedContacts == null)
                {
                    _participatedContacts = Endpoint.GetContactIdsByParticipationStatus(Activity, false);
                }
                return _participatedContacts;
            }
        }

        protected void ReloadParticipatedContacts() 
        {
            _participatedContacts = null;
        }
        #endregion

    }
}
