using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Excelta.NKrusted.Core
{
    /// <summary>
    /// This is the attribute of the primary key, it is always int and auto incremental (simplified model)
    /// </summary>
    /// 

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute:Attribute
    {
        public PrimaryKeyAttribute(string mappedColumnName)
        {
            _mappedColumnName = mappedColumnName;
        }

        public PrimaryKeyAttribute(string mappedColumnName, string mappedTableName)
        {
            _mappedColumnName = mappedColumnName;
            _mappedTableName = mappedTableName;
            _isTableNameOnClass = false;
        }

        protected string _mappedTableName = null;
        protected bool _isTableNameOnClass = true;
        protected string _mappedColumnName;
        public string MappedColumnName
        {
            get { return _mappedColumnName; }
        }

        public string MappedTableName { get { return _mappedTableName; } }
    }
}
