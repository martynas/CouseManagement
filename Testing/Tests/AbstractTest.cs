using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace Testing.Tests
{
    abstract class AbstractTest
    {
        /// <summary>
        /// Open connection to the server.
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["Membership_DB"];
            SqlConnection conn = new SqlConnection(settings.ConnectionString);
            conn.Open();
            return conn;
        } 
    }
}
