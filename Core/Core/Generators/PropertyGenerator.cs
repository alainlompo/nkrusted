using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core.Generators
{
    public class PropertyGenerator
    {
        private string _propertyName;
        private List<AttributeGenerator> _attributeList;
        private bool _hasGetter;
        private bool _hasSetter;
        private DataTypes _propertyType;
        public PropertyGenerator(string propertyName,DataTypes propertyType, List<AttributeGenerator> attribsList, bool hasGetter, bool hasSetter)
        {
            _propertyName = propertyName;
            _attributeList = attribsList;
            _hasGetter = hasGetter;
            _hasSetter = hasSetter;
            _propertyType = propertyType;
        }



        public string GetGeneratedProperty()
        {
            string result = null;
            string typeName = null;
            switch (_propertyType)
            {
                case DataTypes.Boolean:
                    typeName = "bool";
                    break;
                case DataTypes.Decimal:
                    typeName = "double";
                    break;
                case DataTypes.Integer:
                    typeName = "int";
                    break;
                case DataTypes.String:
                    typeName = "string";
                    break;
                default:
                    typeName = "string";
                    break;
            }
            StringBuilder builder = new StringBuilder();

            builder.Append("private " + typeName + " _" + _propertyName + ";\n");
            foreach (AttributeGenerator attrG in _attributeList)
            {
                builder.Append(attrG.GetGeneratedAttribute() + "\n");
            }

            builder.Append("public " + typeName + " " + _propertyName + "\n");
            builder.Append("{\n");
            if (_hasGetter)
            {
                builder.Append(" get { return _" + _propertyName + ";}\n");

            }
            if (_hasSetter)
            {
                builder.Append(" set { _" + _propertyName + " = value; }\n");
            }
            builder.Append("}\n");

            result = builder.ToString();


            return result;




        }
    }
}
