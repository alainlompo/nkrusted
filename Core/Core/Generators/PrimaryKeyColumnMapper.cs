using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core.Generators
{
    public class PrimaryKeyColumnMapper
    {
        /// <summary>
        /// This is a primary key column, not an ordinary column
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public PropertyGenerator GetGeneratorFrom(string columnName, string dataType)
        {
            PropertyGenerator result = null;


            /////////////////////////////////////////////////
            /// The current version assumes that 
            /// primary key is auto - incremental int
            /// //////////////////////////////////////////////

            DataTypes type = DataTypes.String;
            if (dataType.ToUpper().Trim().Equals("INT"))
                type = DataTypes.Integer;
            if (dataType.ToUpper().Trim().Equals("BIT"))
                type = DataTypes.Boolean;
            if (dataType.ToUpper().Trim().Equals("DECIMAL"))
                type = DataTypes.Decimal;

            Dictionary<string, object> dicoParams = new Dictionary<string, object>();
            dicoParams.Add("p1", "\"" + columnName + "\"");
            //////////////dicoParams.Add("p2", "DataTypes." + type);

            AttributeGenerator colAttrGen = new AttributeGenerator("PrimaryKey", dicoParams);
            List<AttributeGenerator> list = new List<AttributeGenerator>();
            list.Add(colAttrGen);

            result = new PropertyGenerator("P_" + columnName, type, list, true, true);


            return result;
        }
    }
}
