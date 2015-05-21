using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core.Generators
{
    public class AttributeGenerator
    {
        private string _attributeName;
        private Dictionary<string, object> _parametersList;

        public AttributeGenerator(string attrName, Dictionary<string, object> parameters)
        {
            _attributeName = attrName;
            _parametersList = parameters;

        }

        /// <summary>
        /// Using this method if you want to insert a string you need
        /// to add it with its double quote at the beginning and end;
        /// </summary>
        /// <returns></returns>
        public string GetGeneratedAttribute()
        {
            string result = null;
            StringBuilder builder = new StringBuilder();
            builder.Append("[")
                .Append(_attributeName)
                .Append("(");
            object oValue = null;
            bool isFirst = true;
            foreach (string s in _parametersList.Keys)
            {
                if (!isFirst)
                    builder.Append(",");

                if (_parametersList.TryGetValue(s, out oValue))
                {
                    builder.Append(oValue);
                }
                isFirst = false;


                
            }
            builder.Append(")]");


            result = builder.ToString();





            return result;
        }
    }
}
