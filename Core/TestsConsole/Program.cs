using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excelta.NKrusted;
using Excelta.NKrusted.Core;
using Excelta.NKrusted.Core.Examples;
using System.Data;

namespace TestsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Excelta.NKrusted.Core.PersistanceManager pm = new Excelta.NKrusted.Core.PersistanceManager();

            // ********************* First test: creation of datas **********************************************
            
            Excelta.NKrusted.Core.Examples.Employee emp = new Excelta.NKrusted.Core.Examples.Employee("Sani10", "Abacha");
            pm.Add(emp);
            bool x = true;
            string y = x.ToString();
            Console.WriteLine(y);




            NKrustedDAO.C_Employees emp22 = new NKrustedDAO.C_Employees();
            emp22.P_FName = "Clement";
            emp22.P_LName = "Lompo";
            pm.Add(emp22);




            Console.ReadKey();

            // ******************** Second test: retrieve datas by Id *****************************************



            Employee emp2 = new Employee();
            emp2.IdEmployee = 5;

            DataTable dt = (DataTable)pm.GetDatas(emp2);
            foreach (DataColumn dc in dt.Columns)
            {
                Console.Write(dc.ColumnName + "::::::");
            }
            Console.WriteLine();
            
            foreach (DataRow dr in dt.Rows) {

                for (int i = 0; i < dt.Columns.Count; i++)
                    Console.Write(dr[i].ToString() + " ");
                Console.WriteLine();
                 
             }


            Employee emp3 = (Employee)pm.Get(emp2);
            Console.WriteLine(emp3.IdEmployee);
            Console.WriteLine(emp3.FirstName);
            Console.WriteLine(emp3.LastName);

            /*Employee emp4 = new Employee();
            emp4.IdEmployee = emp3.IdEmployee;
            emp4.FirstName = "Alain";
            emp4.LastName = "Lompo";

            pm.Update(emp4);
            */

            /*Employee emp5 = new Employee("Souley", "Brah");
            pm.Add(emp5);
            */
            Console.ReadKey();






        }
    }
}
