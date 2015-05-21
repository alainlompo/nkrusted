using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;

namespace Excelta.NKrusted.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectionAttribute:Attribute
    {
        /// <summary>
        /// This constructor create a new SqlConnection object and register it with the ConnectionsManager
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="connectionID"></param>
        public ConnectionAttribute(string connectionString, string connectionID)
        {
            _connectionString = connectionString;
            _connectionID = connectionID;

            ConnectionsManager.Put(connectionID, new System.Data.SqlClient.SqlConnection(connectionString));

        }


        /// <summary>
        /// This constructor tries to get a connection from the manager with the connectionID, in case of failure an exception
        /// if thrown
        /// </summary>
        /// <param name="connectionID"></param>
        public ConnectionAttribute(string connectionID)
        {
            SqlConnection con = ConnectionsManager.Get(connectionID);
            if (con == null)
                throw new UnRegisteredConnectionException("No such connection in the connections dictionnary");
            _connectionString = con.ConnectionString;
            _connectionID = connectionID;
           
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            
        }

        protected string _connectionString;
        protected string _connectionID;

        public string ConnectionID
        {
            get { return _connectionID; }
        }

    }
}
