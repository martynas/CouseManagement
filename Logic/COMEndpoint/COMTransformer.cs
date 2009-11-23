using System;
using System.Collections.Generic;
using System.Text;
using LWM = AstonRDLightWeightModel;

namespace Atrendia.CourseManagement.Logic.COMEndpoint
{
    /// <summary>
    /// Loads objects from CDM objects/lists etc. to our entities/lists.
    /// </summary>
    class COMTransformer : IDisposable
    {
        COMEndpoint endpoint;

        public COMTransformer(COMEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        /// <summary>
        /// Helper for setting CDM object fields.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal static void CDMSet(LWM.CDMObject obj, string key, object value)
        {
            object placeholder = value;
            obj.set_Attrib(key, false, ref placeholder);
        }

        #region IDisposable Members
        public void Dispose()
        {
            this.endpoint = null;
        }
        #endregion

        #region Load
        public Entities.Contact Load(LWM.CDMObject cdm, Entities.Contact contact)
        {
            contact.Id = cdm.Id;
            contact.Title = (string)cdm.get_Attrib("title", false);
            contact.FirstName = (string)cdm.get_Attrib("firstName", false);
            contact.LastName = (string)cdm.get_Attrib("lastName", false);
            contact.Email = (string)cdm.get_Attrib("email", false);
            contact.DirectPhone = (string)cdm.get_Attrib("directPhone", false);
            contact.MobilePhone = (string)cdm.get_Attrib("mobilePhone", false);
            contact.PrimaryCompanyID = (string)cdm.get_Attrib("primaryCompany", false);
            return contact;
        }

        public Entities.Company Load(LWM.CDMObject cdm, Entities.Company company)
        {
            company.Id = cdm.Id;
            company.Alias = (string)cdm.get_Attrib("alias", false);
            return company;
        }

        public Entities.DeliveryPackage Load(LWM.CDMObject cdm, Entities.DeliveryPackage package)
        {
            package.Id = cdm.Id;
            package.Heading = (string)cdm.get_Attrib("heading", false);
            package.GroupSize = (int)((double)cdm.get_Attrib("groupSize", false));
            package.TrainingSessions = (int)((double)cdm.get_Attrib("trainingSessions", false));
            return package;
        }

        public Entities.User Load(LWM.CDMObject cdm, Entities.User user)
        {
            user.Id = cdm.Id;
            user.Title = (string)cdm.get_Attrib("title", false);
            user.FirstName = (string)cdm.get_Attrib("firstName", false);
            user.LastName = (string)cdm.get_Attrib("lastName", false);
            user.DirectPhone = (string)cdm.get_Attrib("directPhone", false);
            user.MobilePhone = (string)cdm.get_Attrib("mobilePhone", false);
            user.Email = (string)cdm.get_Attrib("email", false);
            return user;
        }

        public Entities.ProductGroup Load(LWM.CDMObject cdm, Entities.ProductGroup pg)
        {
            pg.Id = cdm.Id;
            pg.Heading = (string)cdm.get_Attrib("heading", false);
            return pg;
        }

        public Entities.Activity Load(LWM.CDMObject cdm, Entities.Activity activity)
        {
            activity.Id = cdm.Id;
            activity.Heading = (string)cdm.get_Attrib("heading", false);
            activity.Notes = cdm.get_Comments(2);
            activity.IsDone = (bool)cdm.get_Attrib("activityDone", false);
            activity.Date = ((DateTime)cdm.get_Attrib("startDate", false)).Date;
            activity.TrainingLocation = (string)cdm.get_Attrib("trainingLocation", false);
            activity.PrimaryCompanyId = (string)cdm.get_Attrib("primaryCompany", false);
            activity.PrimaryContactId = (string)cdm.get_Attrib("primaryContact", false);
            if (!(cdm.get_Attrib("startTime", false) is DBNull))
                activity.StartTime = ((DateTime)cdm.get_Attrib("startTime", false)).TimeOfDay;
            if (!(cdm.get_Attrib("endTime", false) is DBNull))
                activity.EndTime = ((DateTime)cdm.get_Attrib("endTime", false)).TimeOfDay;

            // Primary delivery package ID
            object primaryDeliveryPackage = cdm.get_Attrib("primaryDeliveryPackage", false);
            if (!(primaryDeliveryPackage is DBNull))
                activity.DeliveryPackageId = (string)primaryDeliveryPackage;

            // Type information
            activity.TypeFlag = Entities.ActivityTypeFlag.None;
            if (!(cdm.get_Attrib("primaryActivityType", false) is DBNull))
            {
                LWM.CDMObject type = cdm.get_Relation("primaryActivityType");
                if ((bool)type.get_Attrib("courseAct", false))
                    activity.TypeFlag = Entities.ActivityTypeFlag.Course;
                else if ((bool)type.get_Attrib("deliveryPreparation", false))
                    activity.TypeFlag = Entities.ActivityTypeFlag.Preparation;
            }

            // Tranining contents
            if (!(cdm.get_Attrib("primaryTrainingContents", false) is DBNull))
            {
                LWM.CDMObject contents = cdm.get_Relation("primaryTrainingContents");
                activity.ContentsHeading = (string)contents.get_Attrib("heading", false);
                activity.ContentsNotes = contents.get_Comments(1);
            }

            //E-mail client
            if (!(cdm.get_Attrib("outlookVersion", false) is DBNull))
            {
                LWM.CDMObject contents = cdm.get_Relation("outlookVersion");
                activity.EmailPlatform = (string)contents.get_Attrib("heading", false);
            }

            //Add-in Version
            if (!(cdm.get_Attrib("primaryAddinVersion", false) is DBNull))
            {
                LWM.CDMObject addin = cdm.get_Relation("primaryAddinVersion");
                activity.AddinVersion = (string)addin.get_Attrib("heading", false);
            }

            //Language
            if (!(cdm.get_Attrib("primaryLanguage", false) is DBNull))
            {
                LWM.CDMObject language = cdm.get_Relation("primaryLanguage");
                activity.Language = (string)language.get_Attrib("heading", false);
            }

            return activity;
        }
        #endregion

        #region Store
        public LWM.CDMObject Store(LWM.CDMObject cdm, Entities.Contact contact)
        {
            CDMSet(cdm, "title", contact.Title);
            CDMSet(cdm, "firstName", contact.FirstName);
            CDMSet(cdm, "lastName", contact.LastName);
            CDMSet(cdm, "email", contact.Email);
            CDMSet(cdm, "directPhone", contact.DirectPhone);
            CDMSet(cdm, "mobilePhone", contact.MobilePhone);
            CDMSet(cdm, "primaryCompany", contact.PrimaryCompanyID);
            return cdm;
        }
        #endregion
    }
}
