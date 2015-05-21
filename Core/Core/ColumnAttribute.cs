using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Excelta.NKrusted.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute:Attribute
    {
        public ColumnAttribute(string mappedColumnName, DataTypes dataType)
        {
            _dataType = dataType;
            _mappedColumnName = mappedColumnName;

        }

        public ColumnAttribute(string mappedColumnName, DataTypes dataType, string mappedTableName)
        {
            _mappedColumnName = mappedColumnName;
            _dataType = dataType;
            _mappedTableName = mappedTableName;
            _isTableNameOnClass = false;
        }

        protected string _mappedTableName = null;
        protected bool _isTableNameOnClass = true;

        protected string _mappedColumnName;
        public string MappedColumnName { get { return _mappedColumnName; } }
        protected DataTypes _dataType;
        public DataTypes DataType { get { return _dataType; } }
        public string MappedTableName { get { return _mappedTableName; } }

    }
}
