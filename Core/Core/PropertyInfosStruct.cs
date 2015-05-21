using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core
{
    // mappedname, value, type
    public struct PropertyInfosStruct
    {
        public string mappedColumnName;
        public string propertyValue;
        public DataTypes propertyDataType;

        public PropertyInfosStruct(string mappedName, string propValue, DataTypes dataType)
        {
            mappedColumnName = mappedName;
            propertyValue = propValue;
            propertyDataType = dataType;

        }

    }
}
