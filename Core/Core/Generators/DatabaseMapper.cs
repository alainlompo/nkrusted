using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excelta.NKrusted.Core.Metadatas;
using System.Data;
using System.IO;

namespace Excelta.NKrusted.Core.Generators
{
    public class DatabaseMapper
    {


        public void GenerateClassFiles(string classesPath, string connectionString, string databaseName)
        {
            List<ClassGenerator> liste = GetAllGeneratedClassesFrom(connectionString, databaseName);
            foreach (ClassGenerator cg in liste)
            {
                
                StreamWriter outFile = new StreamWriter(classesPath + "\\" + cg.GetClassName() + ".cs");
                outFile.Write(cg.GetGeneratedClass());
                outFile.Close();
            }
        }
        public List<ClassGenerator> GetAllGeneratedClassesFrom(string connectionString, string databaseName)
        {
            List<ClassGenerator> liste = new List<ClassGenerator>();

            SqlServerMetadatas sqlMetas = new SqlServerMetadatas();
            DataTable baseTables = sqlMetas.GetSqlServerSchemas(connectionString);
            ClassTableMapper cTM = new ClassTableMapper();

            foreach (DataRow dr in baseTables.Rows)
            {
                liste.Add( cTM.GetGeneratorFrom(connectionString,databaseName,dr["TABLE_NAME"].ToString()));



            }




            return liste;
        }
    }
}
