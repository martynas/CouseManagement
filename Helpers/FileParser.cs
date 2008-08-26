using System;
using System.IO;
using System.Collections.Generic;

namespace Atrendia.CourseManagement.Helpers
{
    /// <summary>
    /// Defines parsing error
    /// </summary>
    public class ParsingError
    {
        private int lineNumber;
        private string rawLine;
        private string message;

        public ParsingError(int lineNumber, string rawLine, string message)
        {
            this.lineNumber = lineNumber;
            this.rawLine = rawLine;
            this.message = message;
        }

        #region Properties
        public int LineNumber
        {
            get { return lineNumber; }
        }
        public string RawLine
        {
            get { return rawLine; }
        }
        public string Message
        {
            get { return message; }
        }
        #endregion
    }

    public class ContactFileParser : IDisposable
    {
        private List<ParsingError> errors;
        private List<Logic.Entities.Contact> contacts; 
        private Stream stream;

        public ContactFileParser(Stream stream)
        {
            this.stream = stream;
            this.errors = new List<ParsingError>();
            this.contacts = new List<Logic.Entities.Contact>();
        }

        /// <summary>
        /// Parse input stream into contacts while collecting
        /// parsing errors.
        /// </summary>
        /// <returns></returns>
        public List<Logic.Entities.Contact> Parse()
        {
            int lineNumber = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                char separator = ';';
                while ((line = reader.ReadLine()) != null)
                {
                    ++lineNumber;
                    line = line.Trim();
                    // Choose field separator
                    if (lineNumber == 1)
                    {
                        if (line.IndexOf(';') == -1)
                        {
                            separator = ','; // Common in Scandinavian countries
                        }
                        string[] titles = line.Split(separator);
                        if (titles.Length != 5)
                        {
                            errors.Add(new ParsingError(lineNumber, line,
                                String.Format("Expected 5 title fields, got {0}, stopping",
                                    titles.Length)));
                            break;
                        }
                    }
                    else
                    {
                        if (line.Length == 0) // Ignore empty lines
                        {
                            continue;
                        }
                        string[] fields = line.Split(separator);
                        if (fields.Length != 5)
                        {
                            errors.Add(new ParsingError(lineNumber, line,
                                String.Format("Expected 5 fields, got {0}", fields.Length)));
                            continue;
                        }
                        for (int i = 0; i < fields.Length; i++)
                        {
                            fields[i] = fields[i].Trim();
                        }
                        // Fill in contact
                        Logic.Entities.Contact contact = new Logic.Entities.Contact();
                        contact.Title = fields[0];
                        contact.FirstName = fields[1];
                        contact.LastName = fields[2];
                        contact.Email = fields[3];
                        contact.MobilePhone = fields[4];
                        contacts.Add(contact);
                    }
                    // No title row
                    if (lineNumber == 0)
                    {
                        errors.Add(new ParsingError(1, string.Empty,
                            "No data rows in uploaded file"));
                    }
                }

            }
            return contacts;
        }

        public List<ParsingError> Errors
        {
            get { return errors; }
        }

        #region IDisposable Members
        public void Dispose()
        {
            stream.Close();
        }
        #endregion
    }
}