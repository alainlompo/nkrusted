using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Excelta.NKrusted.Core.Examples
{
    /// <summary>
    /// Classe Employee annoté des attributs Connection et Table, ainsi on sait ou la persister.
    /// </summary>
    [Connection(@"server=LOMPO-PC\SQL2000PE; initial catalog = NKRUSTEDDEMO; user id = sa; password = loverofmysoul", "nkrustedcon")]
    [Table("Employees")]
    public class Employee
    {
        private int _idEmployee;
        

        /// <summary>
        /// Propriété annotée de l'attribut PrimaryKey, ainsi on sait que c'est une clef primaire
        /// </summary>
        [PrimaryKey("Id")]
        public int IdEmployee
        {
            get { return _idEmployee; }
            set { _idEmployee = value; }

        }


        private string _firstName;
    
        [Column("FName",DataTypes.String)]
        public string FirstName { get { return _firstName; } set { _firstName = value; } }



        private string _lastName = "LOMPO";

        [Column("LName", DataTypes.String)]
        public string LastName { get { return _lastName; } set { _lastName = value; } }

        public Employee()
        {

        }
        public Employee(string fName, string lName)
        {
            _firstName = fName;
            _lastName = lName;
        }

        public override string ToString()
        {
            return "(" + _firstName + " " + _lastName + ")";
        }

    }
}
