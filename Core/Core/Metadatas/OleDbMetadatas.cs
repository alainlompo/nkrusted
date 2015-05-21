using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Excelta.NKrusted.Core.Metadatas
{
    public class OleDbMetadatas
    {
        public DataTable GetTablesFrom(string oledbConnectionString)
        {
            OleDbConnection con = new OleDbConnection(oledbConnectionString);
            con.Open();
            object[] objectArrRest = new object[] { null, null, null, "TABLE" };
            DataTable schemaTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, objectArrRest);
            return schemaTable;


        }

        public DataTable GetColumnsFrom(string oleDbConnectionString, string tableName)
        {
            OleDbConnection con = new OleDbConnection(oleDbConnectionString);
            con.Open();
            object[] objArrRestrict = new object[] { null, null, tableName, null };
            DataTable schemaCols;
            schemaCols = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, objArrRestrict);
            return schemaCols;


        }

        public DataTable GetSqlServerColumnsFrom(string serverInfos, string initialCatalog, string userId, string passWord, string tableName)
        {
            return GetColumnsFrom(GetSqlOleDbConnectionStringFrom(serverInfos, initialCatalog, userId, passWord), tableName);


        }

        public string GetSqlOleDbConnectionStringFrom(string serverInfos, string initialCatalog, string userId, string passWord)
        {
            string connectionString = "Provider=SQLOLEDB; Data Source=" + serverInfos + "; initial catalog=" + initialCatalog + ";";
            string secondPart = (userId == null) ? "Trusted_Connection=Yes;" : "user id =" + userId + "; password=" + passWord;


            connectionString = connectionString + secondPart;
            return connectionString;
        }


        
        public DataTable GetSqlServerTablesFrom(string serverInfos, string initialCatalog, string userId, string passWord)
        {
            string connectionString = "Provider=SQLOLEDB; Data Source=" + serverInfos + "; initial catalog=" + initialCatalog + ";";
            string secondPart = (userId == null) ? "Trusted_Connection=Yes;" : "user id =" + userId + "; password=" + passWord;


            connectionString = connectionString + secondPart;
            return GetTablesFrom(connectionString);

        }


    }
}
