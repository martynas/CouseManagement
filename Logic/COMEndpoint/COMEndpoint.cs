using System;
using System.Collections.Generic;
using System.Text;
using LWM = AstonRDLightWeightModel;

namespace Atrendia.CourseManagement.Logic.COMEndpoint
{
    /// <summary>
    /// COM-powered CDM endpoint. See IEndpoint for more information.
    /// </summary>
    public partial class COMEndpoint : IEndpoint
    {
        private string applicationName;
        private COMTransformer transformer;
        private LWM.Application application;

        // Class cache
        private Dictionary<string, LWM.Class> _classCache;

        /// <summary>
        /// Instantiate COM-powered endpoint instance.
        /// </summary>
        /// <param name="applicationName"></param>
        public COMEndpoint(string applicationName)
        {
            this.applicationName = applicationName;
            this._classCache = new Dictionary<string, LWM.Class>();
            this.transformer = new COMTransformer(this);
            this.OpenApplication();
        }

        #region LWM machinery
        /// <summary>
        /// Open LWM application.
        /// </summary>
        private void OpenApplication()
        {
            application = new LWM.ApplicationClass();
            application.OpenApplication(this.applicationName);
        }

        /// <summary>
        /// Retrieve LWM Class definition, use cache.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private LWM.Class GetClass(string name)
        {
            string canonical = name.ToLowerInvariant();
            if (!_classCache.ContainsKey(canonical))
            {
                _classCache[canonical] = application.Class(name);
            }
            return _classCache[canonical];
        }

        /// <summary>
        /// LWM Class that represents STDContact.
        /// </summary>
        private LWM.Class ClassContact
        {
            get { return GetClass("STDContact"); }
        }

        /// <summary>
        /// LWM Class that represents STDCompany.
        /// </summary>
        private LWM.Class ClassCompany
        {
            get { return GetClass("STDCompany"); }
        }

        /// <summary>
        /// LWM class that represents DeliveryPackage.
        /// </summary>
        private LWM.Class ClassDeliveryPackage
        {
            get { return GetClass("DeliveryPackage"); }
        }

        /// <summary>
        /// LWM class that represents DeliveryPackageContact.
        /// </summary>
        private LWM.Class ClassDeliveryPackageContact
        {
            get { return GetClass("DeliveryPackageContact"); }
        }
        
        /// <summary>
        /// LWM class that represents STDTargetGroup.
        /// </summary>
        private LWM.Class ClassTargetGroup
        {
            get { return GetClass("STDTargetGroup"); }
        }

        /// <summary>
        /// LWM class that represents STDTargetGroup2Contact.
        /// </summary>
        private LWM.Class ClassTargetGroupContact
        {
            get { return GetClass("STDTargetGroup2Contact"); }
        }

        /// <summary>
        /// LWM class that represents STDActivity.
        /// </summary>
        private LWM.Class ClassActivity
        {
            get { return GetClass("STDActivity"); }
        }

        /// <summary>
        /// LWM class that represents STDActivity.
        /// </summary>
        private LWM.Class ClassUser
        {
            get { return GetClass("User"); }
        }

        /// <summary>
        /// LWM class that represents STDActivityType.
        /// </summary>
        private LWM.Class ClassActivityType
        {
            get { return GetClass("STDActivityType"); }
        }

        /// <summary>
        /// LWM class that represents Participant<>Course.
        /// </summary>
        private LWM.Class ClassParticipantCourse
        {
            get { return GetClass("ParticipantCourse"); }
        }

        /// <summary>
        /// LWM class that represents STDProduct.
        /// </summary>
        private LWM.Class ClassProduct
        {
            get { return GetClass("STDProduct"); }
        }

        /// <summary>
        /// LWM class that represents CDMAssignedContactRole.
        /// </summary>
        private LWM.Class ClassCDMAssignedContactRole
        {
            get { return GetClass("CDMAssignedContactRole"); }
        }

        /// <summary>
        /// LWM class that represents CDMSecurityRole2User.
        /// </summary>
        private LWM.Class ClassCDMSecurityRole2User
        {
            get { return GetClass("CDMSecurityRole2User"); }
        }

        /// <summary>
        /// LWM class that represents STDProductGroup.
        /// </summary>
        private LWM.Class ClassSTDProductGroup
        {
            get { return GetClass("TrainingContent"); }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            transformer.Dispose();
            application.CloseApplication();
        }
        #endregion

        #region Contacts and Companies
        /// <summary>
        /// Retrieve specific contact.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Contact GetContactById(string id)
        {
            LWM.CDMObject cdm = ClassContact.LoadObject(id, true);
            return cdm != null ? transformer.Load(cdm, new Entities.Contact()) : null;
        }

        /// <summary>
        /// Retrieve contact by email. May return null.
        /// If company is supplied, contact search is limited to that company.
        /// TODO: when no company is given and multiple contacts are be found
        ///     we should raise exception or similar.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="checkCompany"></param>
        /// <returns></returns>
        public Entities.Contact GetContactByEmail(string email, Entities.Company company)
        {
            LWM.CDMObject cdm;
            if (company != null)
            {
                cdm = ClassContact.LoadFrom(
                        new string[] { "email", "primaryCompany" },
                        new string[] { email, company.Id },
                        true, true
                    );
            }
            else
            {
                cdm = ClassContact.LoadFrom(
                        new string[] { "email" },
                        new string[] { email },
                        true, true
                    );
            }
            return cdm != null ? transformer.Load(cdm, new Entities.Contact()) : null;
        }


        /// <summary>
        /// Tries to resolve Contacts by their Emails
        /// </summary>
        /// <param name="Contacts"></param>
        /// <param name="Resolved"></param>
        /// <param name="NotResolved"></param>
        public void ResolveContactIdByEmail(Entities.Company company, ref IList<Entities.Contact> Contacts,
            out IList<Entities.Contact> Resolved, out IList<Entities.Contact> NotResolved)
        {
            Resolved = new List<Entities.Contact>();
            NotResolved = new List<Entities.Contact>();
            List<Entities.Contact> all = new List<Entities.Contact>();

            foreach (Entities.Contact c in Contacts)
            {
                Entities.Contact cdm = GetContactByEmail(c.Email, company);
                if (cdm != null)
                {
                    all.Add(cdm);
                    Resolved.Add(cdm);
                }
                else
                {
                    all.Add(c);
                    NotResolved.Add(c);
                }
            }

            Contacts = all;
        }

        /// <summary>
        /// Retrieve all contacts from the company.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Entities.Contact> GetContactsByCompanyId(string id)
        {
            List<Entities.Contact> contacts = new List<Entities.Contact>();
            LWM.List query = ClassContact.NewList();
            query.AddWherePart("primaryCompany-.id", id, "=");
            query.ShowDeleted = false;
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                contacts.Add(transformer.Load(cdm, new Entities.Contact()));
                query.MoveNext();
            }
            contacts.Sort(new Entities.ContactLexicographicalComparer());
            return contacts;
        }

        public List<Entities.Contact> GetContactsInRole(string roleCode)
        {
            List<Entities.Contact> contacts = new List<Entities.Contact>();
            LWM.List query = ClassCDMAssignedContactRole.NewList();
            query.AddWherePart("primaryContactRole.code", roleCode, "=");
            query.AddWherePart("primaryContact.deleteDate", null, "is");
            query.ShowDeleted = false;
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                contacts.Add(GetContactById((string)cdm.get_Attrib("primaryContact", false)));
                query.MoveNext();
            }
            return contacts;
        }

        /// <summary>
        /// Retrieve company by identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Company GetCompanyById(string id)
        {
            LWM.CDMObject cdm = ClassCompany.LoadObject(id, true);
            return cdm != null ? transformer.Load(cdm, new Entities.Company()) : null;
        }

        /// <summary>
        /// Retrieves company entitiy by given contact.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public Entities.Company GetCompanyByContactId(string contactId)
        {
            LWM.CDMObject cdm = ClassContact.LoadObject(contactId, true);
            if (cdm != null)
            {
                LWM.CDMObject company = cdm.get_Relation("primaryCompany");
                return company != null ? transformer.Load(company, new Entities.Company()) : null;
            }
            return null;
        }

        /// <summary>
        /// Find user responsible for given company.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Entities.User GetAccountManagerByCompanyId(string companyId)
        {
            LWM.CDMObject cdm = ClassCompany.LoadObject(companyId, true);
            if (cdm != null)
            {
                LWM.CDMObject manager = cdm.get_Relation("primaryUser");
                return manager != null ? transformer.Load(manager, new Entities.User()) : null;
            }
            return null;
        }
        
        /// <summary>
        /// Add all contacts from the given list to company. Wrap actions in transaction.
        /// Supplied contacts must be unsaved.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="contacts"></param>
        public void AddContactsToCompany(Entities.Company company, IList<Entities.Contact> contacts)
        {
            LWM.CDMObject cdmCompany = ClassCompany.LoadObject(company.Id, true);
            bool success = true;
            application.BeginTransaction();
            try
            {
                /// Save all contacts in a transaction
                foreach (Entities.Contact contact in contacts)
                {
                    LWM.CDMObject cdmContact = ClassContact.NewObject();
                    cdmContact.set_Relation("primaryCompany", ref cdmCompany);
                    transformer.Store(cdmContact, contact).Update();
                    contact.Id = cdmContact.Id;
                }
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
                /// Rollback/commit depending on the outcome.
                if (success)
                {
                    application.CommitTransaction();
                }
                else
                {
                    application.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// Update contact details in CDM.
        /// </summary>
        /// <param name="contact"></param>
        public void UpdateContact(Entities.Contact contact)
        {
            LWM.CDMObject cdm = ClassContact.LoadObject(contact.Id, true);
            transformer.Store(cdm, contact);
            cdm.Update();
        }

        public void DeleteContacts(List<Entities.Contact> contacts)
        {
            bool success = true;
            application.BeginTransaction();
            try
            {
                foreach (Entities.Contact contact in contacts)
                {
                    LWM.CDMObject cdmContact = ClassContact.LoadObject(contact.Id, true);
                    cdmContact.Delete();
                }
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
                if (success)
                {
                    application.CommitTransaction();
                }
                else
                {
                    application.RollbackTransaction();
                }
            }
        }
        #endregion

        #region CDMUser
        /// <summary>
        /// Retrieve specific CDM User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.User GetCDMUserById(string id)
        {
            LWM.CDMObject cdm = ClassUser.LoadObject(id, true);
            return cdm != null ? transformer.Load(cdm, new Entities.User()) : null;
        }

        public List<Entities.User> GetCDMUsersInRole(string roleCode)
        {
            List<Entities.User> users = new List<Entities.User>();
            LWM.List query = ClassCDMSecurityRole2User.NewList();
            query.AddWherePart("primarySecurityRole.heading", roleCode, "=");
            query.AddWherePart("primaryUser.deleteDate", null, "is");
            query.ShowDeleted = false;
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                users.Add(GetCDMUserById((string)cdm.get_Attrib("primaryUser", false)));
                query.MoveNext();
            }
            return users;
        }
        #endregion

        #region Delivery package
        /// <summary>
        /// Retrieve delivery package by identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.DeliveryPackage GetDeliveryPackageById(string id, Entities.Company company)
        {
            LWM.CDMObject cdm = ClassDeliveryPackage.LoadObject(id, true);
            if (cdm != null)
            {
                if (company != null)
                {
                    LWM.CDMObject actualCompany = cdm.get_Relation("primaryCompany");
                    if (actualCompany == null || company.Id != actualCompany.Id)
                    {
                        return null;
                    }
                }
                return transformer.Load(cdm, new Entities.DeliveryPackage());
            }
            return null;
        }

        /// <summary>
        /// Retrieve account manager for given delivery package.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.User GetAccountManagerByDeliveryPackageId(string id)
        {
            LWM.CDMObject cdm = ClassDeliveryPackage.LoadObject(id, true);
            if (cdm != null)
            {
                LWM.CDMObject manager = cdm.get_Relation("primaryUser");
                return manager != null ? transformer.Load(manager, new Entities.User()) : null;
            }
            return null;
        }

        /// <summary>
        /// Retrieve delivery package list by primary contact (project manager).
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public List<Entities.DeliveryPackage> GetDeliveryPackagesByPrimaryContactId(string contactId)
        {
            List<Entities.DeliveryPackage> packages = new List<Entities.DeliveryPackage>();
            LWM.List query = ClassDeliveryPackage.NewList();
            query.AddWherePart("primaryContact-.id", contactId, "=");
            query.ShowDeleted = false;
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                packages.Add(transformer.Load(cdm, new Entities.DeliveryPackage()));
                query.MoveNext();
            }
            return packages;
        }

        /// <summary>
        /// Retrieve participants of specified delivery package.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Entities.Contact> GetParticipantsByDeliveryPackageId(string id)
        {
            // Fetch from DeliveryPackage2Contact now, but note that
            // target group must contain the very same contacts.
            List<Entities.Contact> participants = new List<Entities.Contact>();
            LWM.List query = ClassDeliveryPackageContact.NewList();
            query.AddWherePart("primaryDeliveryPackage-.id", id, "=");
            query.ShowDeleted = false;
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject deliveryPackageContact = query.GetObject();
                LWM.CDMObject contact = deliveryPackageContact.get_Relation("primaryContact");
                participants.Add(transformer.Load(contact, new Entities.Contact()));
                query.MoveNext();
            }
            participants.Sort(new Entities.ContactLexicographicalComparer());
            return participants;
        }

        /// <summary>
        /// Enlist given contacts (already registered in some company) to delivery
        /// package.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="contacts"></param>
        private void AddParticipantsToDeliveryPackage(Entities.DeliveryPackage package,
                List<Entities.Contact> contacts, bool startTransaction)
        {
            // Here we must add to both DeliveryPackage<>Contact and TargetGroup<>Contact.
            // We are also doing duplicate checks here, but that is really expensive
            // though.
            bool success = true;
            try
            {
                LWM.CDMObject deliveryPackage = ClassDeliveryPackage.LoadObject(package.Id, true);
                LWM.CDMObject targetGroup = deliveryPackage.get_Relation("primaryTargetGroup");
                LWM.CDMObject company = deliveryPackage.get_Relation("primaryCompany");
                if (startTransaction)
                    application.BeginTransaction();
                foreach (Entities.Contact contact in contacts)
                {
                    LWM.CDMObject cdm = ClassContact.LoadObject(contact.Id, true);
                    // DeliveryPackage <> Contact
                    LWM.CDMObject packageRelation = ClassDeliveryPackageContact.LoadFrom(
                        new string[] { "primaryContact", "primaryDeliveryPackage" },
                        new string[] { contact.Id, package.Id },
                        true, true
                    );
                    if (packageRelation == null)
                    {
                        packageRelation = ClassDeliveryPackageContact.NewObject();
                        packageRelation.set_Relation("primaryContact", ref cdm);
                        packageRelation.set_Relation("primaryDeliveryPackage", ref deliveryPackage);
                        packageRelation.Update();
                    }

                    // TargetGroup <> Contact
                    LWM.CDMObject groupRelation = ClassTargetGroupContact.LoadFrom(
                        new string[] { "primaryContact", "primaryTargetGroup" },
                        new string[] { contact.Id, targetGroup.Id },
                        true, true
                    );
                    if (groupRelation == null)
                    {
                        groupRelation = ClassTargetGroupContact.NewObject();
                        groupRelation.set_Relation("primaryContact", ref cdm);
                        groupRelation.set_Relation("primaryTargetGroup", ref targetGroup);
                        groupRelation.set_Relation("primaryCompany", ref company);
                        groupRelation.Update();
                    }
                }
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
                if (startTransaction)
                {
                    if (success)
                    {
                        application.CommitTransaction();
                    }
                    else
                    {
                        application.RollbackTransaction();
                    }
                }
            }
        }

        /// <summary>
        /// Given a delivery package, remove its participants that are
        /// not participants in any of the related activities (i.e. junk).
        /// Note: does not start a new transaction, so must be wrapped
        /// into existing one.
        /// </summary>
        /// <param name="package"></param>
        private void RemoveParticipantsWithNoActivities(Entities.DeliveryPackage package)
        {
            List<Entities.Contact> participants = GetParticipantsByDeliveryPackageId(package.Id);
            Dictionary<string, bool> withActivities = new Dictionary<string, bool>();

            LWM.List query = ClassParticipantCourse.NewList();
            query.AddWherePart("primaryAct.primaryDeliveryPackage.id", package.Id, "=");
            query.ShowDeleted = false;
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject participant = query.GetObject();
                string contactId = (string)participant.get_Attrib("pimaryPart", false);
                withActivities[contactId] = true;
                query.MoveNext();
            }

            // Delete relations
            LWM.CDMObject deliveryPackage = ClassDeliveryPackage.LoadObject(package.Id, true);
            LWM.CDMObject targetGroup = deliveryPackage.get_Relation("primaryTargetGroup");
            LWM.CDMObject company = deliveryPackage.get_Relation("primaryCompany");
            foreach (Entities.Contact contact in participants)
            {
                if (!withActivities.ContainsKey(contact.Id))
                {
                    // DeliveryPackage <> Contact
                    LWM.CDMObject packageRelation = ClassDeliveryPackageContact.LoadFrom(
                        new string[] { "primaryContact", "primaryDeliveryPackage" },
                        new string[] { contact.Id, package.Id },
                        true, true
                    );
                    if (packageRelation != null)
                    {
                        packageRelation.Delete();
                    }

                    // TargetGroup <> Contact
                    LWM.CDMObject groupRelation = ClassTargetGroupContact.LoadFrom(
                        new string[] { "primaryContact", "primaryTargetGroup" },
                        new string[] { contact.Id, targetGroup.Id },
                        true, true
                    );
                    if (groupRelation != null)
                    {
                        groupRelation.Update();
                    }
                }
            }
        }
        #endregion

        #region Activities
        /// <summary>
        /// Retrieve activity object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Activity GetActivityById(string id)
        {
            LWM.CDMObject cdm = ClassActivity.LoadObject(id, true);
            if (cdm != null)
            {
                bool inHouse = (bool)cdm.get_Attrib("inhouse", false);
                if (!inHouse)
                {
                    return transformer.Load(cdm, new Entities.Activity());
                }
            }
            return null;
        }

        /// <summary>
        /// Mark activity as done in CDM.
        /// </summary>
        /// <param name="activity"></param>
        public void MarkActivityAsDone(Entities.Activity activity)
        {
            if (!activity.IsDone)
            {
                try
                {
                    LWM.CDMObject cdm = ClassActivity.LoadObject(activity.Id, true);
                    activity.IsDone = true;
                    COMTransformer.CDMSet(cdm, "activityDone", activity.IsDone);
                    cdm.Update();
                }
                catch (Exception)
                {
                    activity.IsDone = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieve a list of activities in given date range.
        /// </summary>
        /// <param name="deliveryPackageContactId">Returned activities will be
        ///     inside the the delivery packages assigned to this contact.</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<Entities.Activity> GetActivities(string deliveryPackageContactId, 
            int? maxRecords, DateTime startDate, DateTime? endDate)
        {
            List<Entities.Activity> activities = new List<Entities.Activity>();
            LWM.List query = ClassActivity.NewList();
            query.AddOrderByField("startDate", "ASC");
            query.ShowDeleted = false;
            query.AddWherePart("inhouse", false, "=");
            query.AddWherePart("primaryDeliveryPackage-.primaryContact-.id", 
                deliveryPackageContactId, "=");
            query.AddWherePart("startDate", startDate.AddDays(-1), ">");
            if (endDate.HasValue)
                query.AddWherePart("startDate", endDate.Value.AddDays(1), "<");
            if (maxRecords.HasValue)
                query.MaxRecords = maxRecords.Value;
            query.Query(true);
            // Note: we do not filter by activity type. It's up to
            // the receiver; corresponding flags are in Entities.Activity.
            while (!query.EOF)
            {
                LWM.CDMObject activity = query.GetObject();
                activities.Add(transformer.Load(activity, new Entities.Activity()));
                query.MoveNext();
            }
            return activities;
        }

        /// <summary>
        /// Retrieve a list of activities in given date range.
        /// </summary>
        /// <param name="primaryUserId">Returned activities will be assigned to the user</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<Entities.Activity> GetActivitiesByUserId(string primaryUserId,
            int? maxRecords, DateTime startDate, DateTime? endDate, bool includeDone,
            Entities.ActivityTypeFlag? activityType)
        {
            List<Entities.Activity> activities = new List<Entities.Activity>();
            LWM.List query = ClassActivity.NewList();
            query.AddOrderByField("startDate", "ASC");
            query.ShowDeleted = false;
            //query.AddWherePart("inhouse", false, "=");
            query.AddWherePart("primaryUser", primaryUserId, "=");
            query.AddWherePart("primaryDeliveryPackage", null, "is not");
            query.AddWherePart("startDate", startDate.AddDays(-1), ">");
            if (!includeDone)
                query.AddWherePart("activityDone", false, "=");
            if (endDate.HasValue)
                query.AddWherePart("startDate", endDate.Value.AddDays(1), "<");

            if (activityType == Entities.ActivityTypeFlag.Course)
            {
                query.AddWherePart("primaryActivityType.courseAct", true, "=");
            }

            if (maxRecords.HasValue)
                query.MaxRecords = maxRecords.Value;
            query.Query(true);
            // Note: we do not filter by activity type. It's up to
            // the receiver; corresponding flags are in Entities.Activity.
            while (!query.EOF)
            {
                LWM.CDMObject activity = query.GetObject();
                activities.Add(transformer.Load(activity, new Entities.Activity()));
                query.MoveNext();
            }
            return activities;
        }

        /// <summary>
        /// Get trainer (responsible user) for activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public Entities.User GetTrainerForActivity(Entities.Activity activity)
        {
            LWM.CDMObject cdm = ClassActivity.LoadObject(activity.Id, true);
            if (cdm != null)
            {
                LWM.CDMObject trainer = cdm.get_Relation("primaryUser");
                return trainer != null ? transformer.Load(trainer, new Entities.User()) : null;
            }
            return null;
        }

        /// <summary>
        /// Get participants attached to certain activity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Entities.Contact> GetParticipantsByActivityId(string id)
        {
            List<Entities.Contact> participants = new List<Entities.Contact>();

            LWM.List query = ClassParticipantCourse.NewList();
            query.AddWherePart("primaryAct-.id", id, "=");
            query.ShowDeleted = false;
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject participantCourse = query.GetObject();
                // Yes, pimaryPart, nor primaryPart.
                LWM.CDMObject contact = participantCourse.get_Relation("pimaryPart");
                participants.Add(transformer.Load(contact, new Entities.Contact()));
                query.MoveNext();
            }
            participants.Sort(new Entities.ContactLexicographicalComparer());
            return participants;
        }

        /// <summary>
        /// Add given participants to given activity. Duplicates are checked.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="contacts"></param>
        public void AddParticipantsToActivity(Entities.Activity activity, List<Entities.Contact> contacts)
        {
            bool success = true;
            try
            {
                LWM.CDMObject cdmActivity = ClassActivity.LoadObject(activity.Id, true);
                // TODO: there is currently no data about course (product) in
                // CDM. Should be taken either from delivery package or
                // activity, depending on business requirements.
                LWM.CDMObject course = null;

                application.BeginTransaction();
                foreach (Entities.Contact contact in contacts)
                {
                    LWM.CDMObject cdm = ClassContact.LoadObject(contact.Id, true);

                    // Participant <> Course
                    LWM.CDMObject participantRelation = ClassParticipantCourse.LoadFrom(
                        new string[] { "primaryAct", "pimaryPart" },
                        new string[] { activity.Id, contact.Id },
                        true, true
                    );

                    if (participantRelation == null)
                    {
                        participantRelation = ClassParticipantCourse.NewObject();
                        // Yes, pimaryPart, not primaryPart.
                        participantRelation.set_Relation("pimaryPart", ref cdm);
                        participantRelation.set_Relation("primaryAct", ref cdmActivity);
                        if (course != null)
                        {
                            participantRelation.set_Relation("primaryProduct", ref course);
                        }
                        participantRelation.Update();
                    }
                }

                // Add contacts to delivery package as well, but don't
                // start over new transaction
                // TODO: Uncomment following lines in order to get TargetGroup<>Participant filled
                /*if (activity.DeliveryPackageId != null)
                {
                    Entities.DeliveryPackage package = GetDeliveryPackageById(activity.DeliveryPackageId, null);
                    AddParticipantsToDeliveryPackage(package, contacts, false);
                }*/
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
                if (success)
                {
                    application.CommitTransaction();
                }
                else
                {
                    application.RollbackTransaction();
                }
            }    
        }

        /// <summary>
        /// Remove all of the given participants from given activity. Afterwards,
        /// associated DeliveryPackage is also cleaned to remove contacts that are
        /// not associated with any activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="contacts"></param>
        public void RemoveParticipantsFromActivity(Entities.Activity activity, List<Entities.Contact> contacts)
        {
            bool success = true;
            try
            {
                LWM.CDMObject cdmActivity = ClassActivity.LoadObject(activity.Id, true);

                application.BeginTransaction();
                foreach (Entities.Contact contact in contacts)
                {
                    LWM.CDMObject cdm = ClassContact.LoadObject(contact.Id, true);
                    // Participant <> Course
                    LWM.CDMObject participantRelation = ClassParticipantCourse.LoadFrom(
                        new string[] { "primaryAct", "pimaryPart" },
                        new string[] { activity.Id, contact.Id },
                        true, true
                    );
                    if (participantRelation != null)
                    {
                        participantRelation.Delete();
                    }
                }

                // Add contacts to delivery package as well, but don't
                // start over new transaction
                if (activity.DeliveryPackageId != null)
                {
                    Entities.DeliveryPackage package = GetDeliveryPackageById(activity.DeliveryPackageId, null);
                    RemoveParticipantsWithNoActivities(package);
                }
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
                if (success)
                {
                    application.CommitTransaction();
                }
                else
                {
                    application.RollbackTransaction();
                }
            }  
        }

        /// <summary>
        /// Marks that a Contact has/hasn't showed up at an Activity
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="contact"></param>
        /// <param name="noShow"></param>
        public void SetParticipationStatus(Entities.Activity activity, Entities.Contact contact, bool noShow)
        {
            LWM.List query = ClassParticipantCourse.NewList();
            query.ShowDeleted = false;
            query.AddWherePart("primaryAct", activity.Id, "=");
            query.AddWherePart("pimaryPart", contact.Id, "=");
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                COMTransformer.CDMSet(cdm, "noShow", noShow);
                cdm.Update();
                query.MoveNext();
            }
        }

        /// <summary>
        /// Retrieve a list of trainings that given contact went to/
        /// will be going to.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public List<Entities.Activity> GetActivitiesForParticipant(Entities.Contact contact)
        {
            List<Entities.Activity> activities = new List<Entities.Activity>();
            LWM.List query = ClassParticipantCourse.NewList();
            query.AddWherePart("pimaryPart-.id", contact.Id, "=");
            query.ShowDeleted = false;
            query.AddOrderByField("primaryAct-.StartDate", "DESC");
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                // This works against deleted items as well
                Entities.Activity activity = GetActivityById((string)cdm.get_Attrib("primaryAct", false));
                if (activity != null)
                {
                    // TODO: leave courses only
                    activities.Add(activity);
                }
                query.MoveNext();
            }
            return activities;
        }

        /// <summary>
        /// Returns IDs of contacts who have/haven't showed up at the Course
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="noShow"></param>
        /// <returns></returns>
        public List<string> GetContactIdsByParticipationStatus(Entities.Activity activity, bool noShow) 
        {
            List<string> contactIds = new List<string>();
            LWM.List query = ClassParticipantCourse.NewList();
            query.ShowDeleted = false;
            query.AddWherePart("primaryAct", activity.Id, "=");
            query.AddWherePart("noShow", noShow, "=");
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                contactIds.Add((string)cdm.get_Attrib("pimaryPart", false));
                query.MoveNext();
            }
            return contactIds;
        }

        /// <summary>
        /// Sets new activity location
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="trainingLocation"></param>
        public void UpdateTrainingLocation(Entities.Activity activity, string trainingLocation)
        {
            LWM.CDMObject cdm = ClassActivity.LoadObject(activity.Id, true);
            COMTransformer.CDMSet(cdm, "trainingLocation", trainingLocation);
            cdm.Update();
        }

        #endregion

        #region Courses, Products and Modules
        public IList<Entities.ProductGroup> GetAllProductGroups()
        {
            List<Entities.ProductGroup> pgs = new List<Entities.ProductGroup>();
            LWM.List query = ClassSTDProductGroup.NewList();
            query.ShowDeleted = false;
            query.Query(true);
            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                // This works against deleted items as well
                Entities.ProductGroup pg = transformer.Load(cdm, new Entities.ProductGroup());
                if (pg != null)
                {
                    // TODO: leave courses only
                    pgs.Add(pg);
                }
                query.MoveNext();
            }
            return pgs;
        }

        /// <summary>
        /// Return ProductGroup object by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.ProductGroup GetProductGroupById(string id)
        {
            LWM.List query = ClassSTDProductGroup.NewList();
            query.AddWherePart("id", id, "=");
            query.ShowDeleted = false;
            query.Query(true);
            if (!query.EOF)
            {
                return transformer.Load(query.GetObject(), new Entities.ProductGroup());
            }
            else
                return null;
        }

        /// <summary>
        /// Returns a list of courses (ProductGroups) which a contact is willing to take
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>List of courses</returns>
        public VirtualEntities.Contact2ProductGroups GetContact2ProductGroups(Entities.Contact contact)
        {
            VirtualEntities.Contact2ProductGroups c2pg = new VirtualEntities.Contact2ProductGroups(contact);
            LWM.List query = ClassParticipantCourse.NewList();
            query.AddWherePart("primaryTrainingContent", null, "is not");
            query.AddWherePart("pimaryPart", contact, "=");
            query.AddWherePart("primaryAct", null, "is");
            query.ShowDeleted = false;
            query.Query(true);

            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();

                Entities.ProductGroup pg = transformer.Load(cdm.get_Relation("primaryTrainingContent"),
                    new Entities.ProductGroup());
                c2pg.ProductGroups.Add(pg);

                query.MoveNext();
            }

            return c2pg;
        }


        /// <summary>
        /// Updates list of courses which a contact is willing to take
        /// </summary>
        /// <param name="c2pg"></param>
        public void UpdateContact2ProductGroups(VirtualEntities.Contact2ProductGroups c2pg)
        {
            Dictionary<string, Entities.ProductGroup> pgMap = new Dictionary<string, Entities.ProductGroup>();
            foreach (Entities.ProductGroup pg in c2pg.ProductGroups)
            {
                pgMap.Add(pg.Id, pg);
            }

            // Deleting old Contact2ProductGroup relations for the contact
            LWM.List query = ClassParticipantCourse.NewList();
            query.AddWherePart("primaryTrainingContent", null, "is not");
            query.AddWherePart("pimaryPart", c2pg.Contact.Id, "=");
            query.AddWherePart("primaryAct", null, "is");
            query.ShowDeleted = false;
            query.Query(true);

            while (!query.EOF)
            {
                LWM.CDMObject cdm = query.GetObject();
                string pgId = (string)cdm.get_Attrib("primaryTrainingContent", false);
                if (pgMap.ContainsKey(pgId))
                {
                    pgMap.Remove(pgId);
                }
                else
                {
                    // If the contact is no longer wishing to take the Course, the relation is removed
                    cdm.Delete();
                }
                query.MoveNext();
            }

            // Creating new relations
            foreach (string pgId in pgMap.Keys)
            {
                LWM.CDMObject cdm = ClassParticipantCourse.NewObject();
                COMTransformer.CDMSet(cdm, "primaryTrainingContent", pgId);
                COMTransformer.CDMSet(cdm, "pimaryPart", c2pg.Contact.Id);
                cdm.Update();
            }

        }

        public void UpdateContact2ProductGroups(IList<VirtualEntities.Contact2ProductGroups> c2pgInfo)
        {
            foreach (VirtualEntities.Contact2ProductGroups c2pg in c2pgInfo)
            {
                UpdateContact2ProductGroups(c2pg);
            }
        }
        #endregion
    }
}
