using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using LWM = AstonRDLightWeightModel;

namespace Atrendia.CourseManagement.Logic.COMEndpoint
{
    public partial class COMEndpoint
    {

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
            // LWM.CDMObject cdmCompany = ClassCompany.LoadObject(company.Id, true);
            bool success = true;
            application.BeginTransaction();
            try
            {
                /// Save all contacts in a transaction
                foreach (Entities.Contact contact in contacts)
                {
                    contact.PrimaryCompanyID = company.Id;
                    SaveOrUpdateContact(contact);
                    /* LWM.CDMObject cdmContact = ClassContact.NewObject();
                    cdmContact.set_Relation("primaryCompany", ref cdmCompany);
                    transformer.Store(cdmContact, contact).Update();
                    contact.Id = cdmContact.Id; */
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
        public void SaveOrUpdateContact(Entities.Contact contact)
        {
            if (String.IsNullOrEmpty(contact.Id))
            {
                // Creating new Contact
                Debug.Assert(!String.IsNullOrEmpty(contact.PrimaryCompanyID),
                    "Company ID is not set");
                LWM.CDMObject cdmContact = ClassContact.NewObject();
                transformer.Store(cdmContact, contact).Update();
                contact.Id = cdmContact.Id;
            }
            else
            {
                // Updating Existing Contact
                LWM.CDMObject cdm = ClassContact.LoadObject(contact.Id, true);
                transformer.Store(cdm, contact);
                cdm.Update();
            }
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

    }
}
