using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Excelta.NKrusted.Core
{
    public class ConnectionsManager
    {
        private static Dictionary<string, SqlConnection> _connectionsDico = new Dictionary<string, SqlConnection>();

        
        private static int _currentIndex = 0;

        public static void Put(string name, SqlConnection con)
        {
            // Check if this entry already exists in the dictionary
            if (!_connectionsDico.ContainsKey(name))
            {

                _connectionsDico.Add(name, con);
                _currentIndex++;
            }
        }

        public static SqlConnection Get(string name)
        {
            if (_connectionsDico.ContainsKey(name))
                return _connectionsDico[name];
            else return null;

        }


       



    }
}
