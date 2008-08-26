using System;
using System.Collections.Generic;

namespace Atrendia.CourseManagement.Frontend.Support
{
    public class BasePageActivity : Support.BasePage
    {

        #region Properties
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
        }
        #endregion

    }
}
