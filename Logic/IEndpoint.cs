using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic
{
    /// <summary>
    /// This defines the communication interface with CDM. Think of it
    /// as a higher abstraction layer over COM interfaces.
    /// </summary>
    public interface IEndpoint : IDisposable
    {
        #region CDMUser
        Entities.User GetCDMUserById(string id);
        List<Entities.User> GetCDMUsersInRole(string roleCode);
        #endregion

        #region Contacts
        Entities.Contact GetContactById(string id);
        Entities.Contact GetContactByEmail(string email, Entities.Company company);
        List<Entities.Contact> GetContactsByCompanyId(string id);
        void UpdateContact(Entities.Contact contact);
        void DeleteContacts(List<Entities.Contact> contacts);
        List<Entities.Contact> GetContactsInRole(string roleCode);
        void ResolveContactIdByEmail(Entities.Company company, ref IList<Entities.Contact> Contacts,
            out IList<Entities.Contact> Resolved, out IList<Entities.Contact> NotResolved);
        #endregion

        #region Company
        Entities.Company GetCompanyById(string id);
        Entities.Company GetCompanyByContactId(string contactId);
        Entities.User GetAccountManagerByCompanyId(string companyId);
        void AddContactsToCompany(Entities.Company company, IList<Entities.Contact> contacts);
        #endregion

        #region Delivery package
        Entities.DeliveryPackage GetDeliveryPackageById(string id, Entities.Company checkCompany);
        Entities.User GetAccountManagerByDeliveryPackageId(string id);
        List<Entities.DeliveryPackage> GetDeliveryPackagesByPrimaryContactId(string contactId);
        List<Entities.Contact> GetParticipantsByDeliveryPackageId(string id);

        void SetParticipationStatus(Entities.Activity activity, Entities.Contact contact, bool noShow);
        List<string> GetContactIdsByParticipationStatus(Entities.Activity activity, bool noShow);
        #endregion

        #region Activity
        Entities.Activity GetActivityById(string id);
        Entities.User GetTrainerForActivity(Entities.Activity activity);
        void MarkActivityAsDone(Entities.Activity activity);
        List<Entities.Activity> GetActivities(string deliveryPackageContactId,
                int? maxRecords, DateTime startDate, DateTime? endDate);
        List<Entities.Activity> GetActivitiesByUserId(string primaryUserId,
                int? maxRecords, DateTime startDate, DateTime? endDate, bool includeDone,
                Entities.ActivityTypeFlag? activityType);
        List<Entities.Contact> GetParticipantsByActivityId(string id);
        void AddParticipantsToActivity(Entities.Activity activity,
                List<Entities.Contact> contacts);
        void RemoveParticipantsFromActivity(Entities.Activity activity,
                List<Entities.Contact> contacts);
        List<Entities.Activity> GetActivitiesForParticipant(Entities.Contact contact);
        void UpdateTrainingLocation(Entities.Activity activity, string trainingLocation);
        #endregion

        #region Courses, Products and Modules
        IList<Entities.ProductGroup> GetAllProductGroups();
        void UpdateContact2ProductGroups(VirtualEntities.Contact2ProductGroups c2pg);
        void UpdateContact2ProductGroups(IList<VirtualEntities.Contact2ProductGroups> c2pgInfo);
        #endregion

        #region Security & Access Rights
        bool HasAccess(Entities.Contact user, Entities.Contact obj);
        #endregion
    }

    public class ContactRoles
    {
        public const string ProjectManager = "PM";
        public const string Trainer = "Trainer";
    }
}
