using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Excelta.NKrusted.Core
{
	/* This class handles the mapping of an rdbms table */
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute:Attribute
    {
        protected string _mappedTableName;
        /// <summary>
        /// This is the table name as defined in the rdbms
        /// </summary>
        public string MappedTableName { get { return _mappedTableName; } }
        public TableAttribute(string mappedTableName)
        {
            _mappedTableName = mappedTableName;
        }
    }
}
