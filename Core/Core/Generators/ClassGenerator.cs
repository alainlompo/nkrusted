using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core.Generators
{
    public class ClassGenerator
    {
        private List<string> _usingList ;
        private string _namespace;
        private string _className;
        private List<PropertyGenerator> _propertyList;
        private List<AttributeGenerator> _attributesList;

        public string GetClassName() { return _className; }

        public ClassGenerator(List<string> _usList, string nmsp, string clName, List<AttributeGenerator> la, List<PropertyGenerator> lpG)
        {
            _usingList = _usList;
            _namespace = nmsp;
            _className = clName;
            _propertyList = lpG;
            _attributesList = la;

        }

        public string GetGeneratedClass()
        {
            string result = null;

            StringBuilder builder = new StringBuilder();
            foreach (string s in _usingList)
            {
                builder.Append("using " + s + ";\n");
            }

            builder.Append("\n");
            if (_namespace != null)
                builder.Append("namespace " + _namespace + "\n")
                    .Append("{ \n");


            foreach (AttributeGenerator attrG in _attributesList)
            {
                builder.Append(attrG.GetGeneratedAttribute() + "\n");

            }
            builder.Append("public class " + _className + "\n");
            builder.Append("{\n");

            foreach (PropertyGenerator pG in _propertyList)
            {
                builder.Append(pG.GetGeneratedProperty() + "\n\n");
            }

            builder.Append("}\n");






            if (_namespace != null)
                builder.Append("}\n");

            result = builder.ToString();

            return result;
        }



       
    }
}
