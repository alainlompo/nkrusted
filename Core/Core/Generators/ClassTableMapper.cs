using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Excelta.NKrusted.Core.Generators
{
    public class ClassTableMapper
    {
        public bool ContainsColumns(string colName, string dbName, string tbName, DataTable colDatas)
        {
            bool result = false;

            foreach (DataRow dr in colDatas.Rows)
            {
                if (dr["COLUMN_NAME"].ToString().ToUpper().Trim().Equals(colName.Trim().ToUpper()))
                {
                    result = true;
                    break;
                }

            }




            return result;
        }

        public ClassGenerator GetGeneratorFrom(string connectionString, string databaseName, string tableName)
        {
            ClassGenerator result = null;

            // Get the primary key(s)
            Metadatas.SqlServerMetadatas sqlMetas = new Metadatas.SqlServerMetadatas();
            DataTable pKeysTable = sqlMetas.GetSqlServerIndexColumnsFrom(connectionString, databaseName, tableName);
            DataTable columnsTable = sqlMetas.GetSqlServerColumnsFrom(connectionString, databaseName, tableName);

            
            // Iterate over the columns table
            // If find a primaryKey column then generate a PrimaryKeyColumnMapper to get the corresponding propertyGenerator
            // Otherwise generate a PropertyColumnMapper to obtain the good info

            List<PropertyGenerator> progGenList = new List<PropertyGenerator>();
            foreach (DataRow dr in columnsTable.Rows)
            {
                // Create a primary key column mapper
                if (ContainsColumns(dr["COLUMN_NAME"].ToString(), databaseName, tableName, pKeysTable))
                {
                    string colN = dr["COLUMN_NAME"].ToString();
                    string dataType = dr["DATA_TYPE"].ToString();
                    PrimaryKeyColumnMapper pKColMap = new PrimaryKeyColumnMapper();
                    progGenList.Add(pKColMap.GetGeneratorFrom(colN,dataType));
                }
                else 
                {

                    string colN = dr["COLUMN_NAME"].ToString();
                    string dataType = dr["DATA_TYPE"].ToString();
                    PropertyColumnMapper propColMap = new PropertyColumnMapper();
                    progGenList.Add(propColMap.GetGeneratorFrom(colN, dataType));

                }
            }


            
            List<string> nmspList = new List<string>();
            nmspList.Add("System");
            nmspList.Add("System.Collections.Generic");
            nmspList.Add("System.Linq");
            nmspList.Add("System.Text");
            nmspList.Add("Excelta.NKrusted.Core");

            Dictionary<string,object> dico1 = new Dictionary<string,object>();
            dico1.Add("p1", "\"" + connectionString.Replace(@"\",@"\\") + "\"");
            dico1.Add("p2", "\"" + databaseName + "_con" + "\"");


            Dictionary<string,object> dico2 = new Dictionary<string,object>();
            dico2.Add("p2", "\"" + tableName + "\"");

            AttributeGenerator conGen = new AttributeGenerator("Connection",dico1);
            AttributeGenerator tableGen = new AttributeGenerator("Table", dico2);
            List<AttributeGenerator> lA = new List<AttributeGenerator>();
            lA.Add(conGen);
            lA.Add(tableGen);


            result = new ClassGenerator(nmspList, "NKrustedDAO", "C_" + tableName, lA, progGenList);



            return result;
        }
    }
}
