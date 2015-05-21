using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Excelta.NKrusted.Core.Metadatas
{
    public class SqlServerMetadatas
    {
        public DataTable GetSqlServerIndexColumnsFrom(string connectionString, string dataBaseName, string tableName)
        {
            SqlConnection connection =
        new SqlConnection(connectionString);
            string[] restrictions = new string[4] { dataBaseName, null, tableName, null };
            connection.Open();
            DataTable dt = connection.GetSchema("IndexColumns",
                                             restrictions);
            connection.Close();
            return dt;
        }

        public DataTable GetSqlServerColumnsFrom(string connectionString, string dataBaseName, string tableName)
        {
            SqlConnection connection =
        new SqlConnection(connectionString);
            string[] restrictions = new string[4] { dataBaseName, null, tableName, null };
            connection.Open();
            DataTable dt = connection.GetSchema("Columns",
                                             restrictions);
            connection.Close();
            return dt;
        }

        public DataTable GetSqlServerMetaDataCollections(String connectionString)
        {
            SqlConnection connection =
            new SqlConnection(connectionString);
            connection.Open();
            DataTable dt = connection.GetSchema();
            connection.Close();
            return dt;
        }

        public DataTable GetSqlMetaDataFor(string connectionString, string metaDataName)
        {
            SqlConnection connection =
           new SqlConnection(connectionString);
            connection.Open();
            DataTable dt = connection.GetSchema(metaDataName);
            connection.Close();
            return dt;
        }

        public DataTable GetSqlServerSchemas(string connectionString)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Information_Schema.Tables where Table_Type = 'BASE TABLE'", cn);

            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

    }
}
