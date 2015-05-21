using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;


namespace Excelta.NKrusted.Core
{
    public class PersistanceManager:IPersistanceController,IBatchPersistance
    {


        /// <summary>
        /// Retrieve an SqlConnection object from another object's attributes
        /// </summary>
        /// <param name="datasObject"></param>
        /// <returns></returns>
        public SqlConnection GetConnectionFrom(object datasObject)
        {
            SqlConnection _con = null;
            // Obtention du type de l'objet
            Type t = datasObject.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");


            return _con;


        }





        /// <summary>
        /// Add an object into the database, retrieve the details via reflection
        /// </summary>
        /// <param name="datasObject"></param>
        /// <returns></returns>
        public int Add(object datasObject, ref int identityValue)
        {
            int result = -1;
            SqlConnection _con = null;
            string _tableName = "";
            List<PropertyInfosStruct> _mappedColumnsInfos = new List<PropertyInfosStruct>();




            // Obtention du type de l'objet
            Type t = datasObject.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;

            // Gets the list of the columns along with the values that they carry
            PropertyInfo[] objectProperties = (datasObject.GetType()).GetProperties();



            /// For each of these properties info
            /// if it has the column attribute then we store it in a list
            /// And we also store it's mappedColumnName + the Value of the property + the DataType
            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] columnsAttr = pInf.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (columnsAttr != null && columnsAttr.Length > 0)
                {
                    ColumnAttribute colAttr = (ColumnAttribute)columnsAttr[0];
                    object oPropValue = pInf.GetValue(datasObject, null);
                    string sPropValue = (oPropValue != null) ? oPropValue.ToString() : null;

                    PropertyInfosStruct pInfStruct = new PropertyInfosStruct(colAttr.MappedColumnName,
                        sPropValue,
                        colAttr.DataType);

                    _mappedColumnsInfos.Add(pInfStruct);


                }
            }




            // Now we have the connection
            // We have the table name
            // We have the list of the columns and the values and the datatypes

            StringBuilder request = new StringBuilder();

            request.Append(" INSERT INTO ")
                .Append(_tableName)
                .Append(" (");
            for (int i = 0; i < _mappedColumnsInfos.Count - 1; i++)
            {
                request.Append(_mappedColumnsInfos[i].mappedColumnName)
                    .Append(",");
            }

            request.Append(_mappedColumnsInfos[_mappedColumnsInfos.Count - 1].mappedColumnName)
                .Append(") VALUES (");

            for (int i = 0; i < _mappedColumnsInfos.Count - 1; i++)
            {
                string element1 = "";
                string element2 = "";
                string element3 = "";

                DataTypes dType = _mappedColumnsInfos[i].propertyDataType;
                switch (dType)
                {
                    case DataTypes.Boolean:
                        element2 = (_mappedColumnsInfos[i].propertyValue.ToUpper().Equals("TRUE")) ? "1" : "0";
                        break;
                    case DataTypes.Decimal:
                        element2 = _mappedColumnsInfos[i].propertyValue;
                        break;
                    case DataTypes.Integer:
                        element2 = _mappedColumnsInfos[i].propertyValue;
                        break;
                    case DataTypes.String:
                        element1 = "'";
                        element2 = _mappedColumnsInfos[i].propertyValue;
                        element3 = "'";
                        break;
                    default:
                        break;



                }

                request.Append(element1)
                    .Append(element2)
                    .Append(element3)
                    .Append(",");


            }

            string propVal = _mappedColumnsInfos[_mappedColumnsInfos.Count - 1].propertyValue;
            DataTypes propType = _mappedColumnsInfos[_mappedColumnsInfos.Count - 1].propertyDataType;
            string lastchain = "";

            switch (propType)
            {
                case DataTypes.Boolean:
                    lastchain = (propVal.ToUpper().Equals("TRUE")) ? "1" : "0";
                    break;
                case DataTypes.Decimal:
                    lastchain = propVal;
                    break;
                case DataTypes.Integer:
                    lastchain = propVal;
                    break;
                case DataTypes.String:

                    lastchain = "'" + propVal + "'";

                    break;
                default:
                    break;



            }



            request.Append(lastchain)
                .Append(")");
            if (_con.State == ConnectionState.Closed)
                _con.Open();
            SqlCommand cmd = new SqlCommand(request.ToString(), _con);
            result = cmd.ExecuteNonQuery();
            


            StringBuilder request2 = new StringBuilder();

            request2.Append(" SELECT @@Identity as NewId");

            
            SqlCommand cmd2 = new SqlCommand(request2.ToString(), _con);

            cmd2.CommandType = CommandType.Text;
            int result2 = Convert.ToInt32(cmd2.ExecuteScalar());

            if (_con.State == ConnectionState.Open)
                _con.Close();
            identityValue = result2;

            return result;

        }









        /// <summary>
        /// Add an object into the database, retrieve the details via reflection
        /// </summary>
        /// <param name="datasObject"></param>
        /// <returns></returns>
        public int Add(object datasObject)
        {
            int result = -1;
            SqlConnection _con = null;
            string _tableName = "";
            List<PropertyInfosStruct> _mappedColumnsInfos = new List<PropertyInfosStruct>();




            // Obtention du type de l'objet
            Type t = datasObject.GetType();

            MemberInfo mInfos = t;
            
            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;

            // Gets the list of the columns along with the values that they carry
            PropertyInfo[] objectProperties = (datasObject.GetType()).GetProperties();



            /// For each of these properties info
            /// if it has the column attribute then we store it in a list
            /// And we also store it's mappedColumnName + the Value of the property + the DataType
            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] columnsAttr = pInf.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (columnsAttr != null && columnsAttr.Length > 0)
                {
                    ColumnAttribute colAttr = (ColumnAttribute)columnsAttr[0];
                    object oPropValue = pInf.GetValue(datasObject, null);
                    string sPropValue = (oPropValue != null)? oPropValue.ToString():null;

                    PropertyInfosStruct pInfStruct = new PropertyInfosStruct(colAttr.MappedColumnName,
                        sPropValue,
                        colAttr.DataType);

                    _mappedColumnsInfos.Add(pInfStruct);


                }
            }


           

            // Now we have the connection
            // We have the table name
            // We have the list of the columns and the values and the datatypes

            StringBuilder request = new StringBuilder();

            request.Append(" INSERT INTO ")
                .Append(_tableName)
                .Append(" (");
            for (int i = 0; i < _mappedColumnsInfos.Count - 1; i++)
            {
                request.Append(_mappedColumnsInfos[i].mappedColumnName)
                    .Append(",");
            }

            request.Append(_mappedColumnsInfos[_mappedColumnsInfos.Count - 1].mappedColumnName)
                .Append(") VALUES (");

            for (int i = 0; i < _mappedColumnsInfos.Count - 1; i++)
            {
                string element1 = "";
                string element2 = "";
                string element3 = "";

                DataTypes dType = _mappedColumnsInfos[i].propertyDataType;
                switch (dType)
                {
                    case DataTypes.Boolean:
                        element2 = (_mappedColumnsInfos[i].propertyValue.ToUpper().Equals("TRUE")) ? "1" : "0";
                        break;
                    case DataTypes.Decimal:
                        element2 = _mappedColumnsInfos[i].propertyValue;
                        break;
                    case DataTypes.Integer:
                        element2 = _mappedColumnsInfos[i].propertyValue;
                        break;
                    case DataTypes.String:
                        element1 = "'";
                        element2 = _mappedColumnsInfos[i].propertyValue;
                        element3 = "'";
                        break;
                    default:
                        break;



                }

                request.Append(element1)
                    .Append(element2)
                    .Append(element3)
                    .Append(",");


            }

            string propVal = _mappedColumnsInfos[_mappedColumnsInfos.Count - 1].propertyValue;
            DataTypes propType = _mappedColumnsInfos[_mappedColumnsInfos.Count - 1].propertyDataType;
            string lastchain = "";

            switch (propType)
            {
                case DataTypes.Boolean:
                    lastchain = (propVal.ToUpper().Equals("TRUE")) ? "1" : "0";
                    break;
                case DataTypes.Decimal:
                    lastchain = propVal;
                    break;
                case DataTypes.Integer:
                    lastchain = propVal;
                    break;
                case DataTypes.String:
                   
                    lastchain = "'" + propVal + "'";
                    
                    break;
                default:
                    break;



            }



            request.Append(lastchain)
                .Append(")");
            if (_con.State == ConnectionState.Closed)
                _con.Open();
            SqlCommand cmd = new SqlCommand(request.ToString(), _con);
            result = cmd.ExecuteNonQuery();
            _con.Close();


            return result;
           
        }

        public int Delete(object objectToDelete)
        {
            int result = -1;
            SqlConnection _con = null;
            string _tableName = "";
            PropertyInfosStruct _idColumnInfos = new PropertyInfosStruct(null, null, DataTypes.String);

            Type t = objectToDelete.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;


            // Gets the list of the columns along with the values that they carry
            PropertyInfo[] objectProperties = (objectToDelete.GetType()).GetProperties();

            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] idAttrs = pInf.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (idAttrs != null && idAttrs.Length > 0)
                {
                    PrimaryKeyAttribute pKAttr = (PrimaryKeyAttribute)idAttrs[0];
                    object oPropValue = pInf.GetValue(objectToDelete, null);
                    string sPropValue = (oPropValue != null) ? oPropValue.ToString() : null;
                    _idColumnInfos = new PropertyInfosStruct(pKAttr.MappedColumnName, sPropValue, DataTypes.Integer);

                    break;
                }


            }


            StringBuilder request = new StringBuilder();
            request.Append("DELETE ")
                .Append(_tableName)
                .Append(" ")
                .Append("WHERE ")
                .Append(_idColumnInfos.mappedColumnName)
                .Append("=")
                .Append(_idColumnInfos.propertyValue);




            if (_con.State == ConnectionState.Closed)
                _con.Open();
            SqlCommand cmd = new SqlCommand(request.ToString(), _con);
            result = cmd.ExecuteNonQuery();
            _con.Close();


            return result;
        }

        public int Update(object datasHolder)
        {

            int result = -1;
            SqlConnection _con = null;
            string _tableName = "";
            List<PropertyInfosStruct> _mappedColumnsInfos = new List<PropertyInfosStruct>();
            PropertyInfosStruct _idColumnInfos = new PropertyInfosStruct(null, null, DataTypes.String);


            // Obtention du type de l'objet
            Type t = datasHolder.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;

            // Gets the list of the columns along with the values that they carry
            PropertyInfo[] objectProperties = (datasHolder.GetType()).GetProperties();



            /// For each of these properties info
            /// if it has the column attribute then we store it in a list
            /// And we also store it's mappedColumnName + the Value of the property + the DataType
            /// We do the same also for the Id property storing its mappedcolumnname and it's value
            /// because we need it in the update
            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] idAttrs = pInf.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (idAttrs != null && idAttrs.Length > 0)
                {
                    PrimaryKeyAttribute pKAttr = (PrimaryKeyAttribute)idAttrs[0];
                    object oPropValue = pInf.GetValue(datasHolder, null);
                    string sPropValue = (oPropValue != null) ? oPropValue.ToString() : null;
                    _idColumnInfos = new PropertyInfosStruct(pKAttr.MappedColumnName, sPropValue, DataTypes.Integer);
                }

                object[] columnsAttr = pInf.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (columnsAttr != null && columnsAttr.Length > 0)
                {
                    ColumnAttribute colAttr = (ColumnAttribute)columnsAttr[0];
                    object oPropValue = pInf.GetValue(datasHolder, null);
                    string sPropValue = (oPropValue != null) ? oPropValue.ToString() : null;

                    PropertyInfosStruct pInfStruct = new PropertyInfosStruct(colAttr.MappedColumnName,
                        sPropValue,
                        colAttr.DataType);

                    _mappedColumnsInfos.Add(pInfStruct);


                }
            }



            // Now we have the connection,
            // The table
            // The columns
            // The id and value we can go!!!

            StringBuilder request = new StringBuilder();

            request.Append(" UPDATE ")
                .Append(_tableName)
                .Append(" SET ");
            for (int i = 0; i < _mappedColumnsInfos.Count; i++)
            {
                request.Append(_mappedColumnsInfos[i].mappedColumnName)
                    .Append("=");



                string element2 = "";
                

                DataTypes dType = _mappedColumnsInfos[i].propertyDataType;
                switch (dType)
                {
                    case DataTypes.Boolean:
                        element2 = (_mappedColumnsInfos[i].propertyValue.ToUpper().Equals("TRUE")) ? "1" : "0";
                        break;
                    case DataTypes.Decimal:
                        element2 = _mappedColumnsInfos[i].propertyValue;
                        break;
                    case DataTypes.Integer:
                        element2 = _mappedColumnsInfos[i].propertyValue;
                        break;
                    case DataTypes.String:
                        
                        element2 = "'"+_mappedColumnsInfos[i].propertyValue+ "'";
                        
                        break;
                    default:
                        break;



                }

                request.Append(element2);
                if (i < _mappedColumnsInfos.Count - 1)
                    request.Append(",");


            }

            request.Append(" FROM ")
                .Append(_tableName)
                .Append(" WHERE ")
                .Append(_idColumnInfos.mappedColumnName)
                .Append(" = ")
                .Append(_idColumnInfos.propertyValue);


         
            

            if (_con.State == ConnectionState.Closed)
                _con.Open();
            SqlCommand cmd = new SqlCommand(request.ToString(), _con);
            result = cmd.ExecuteNonQuery();
            _con.Close();

            return result;



        }

        /// <summary>
        /// Obtains the object on the basis of the id and returns it
        /// in its own type of datas.
        /// </summary>
        /// <param name="searchCriteriaObject"></param>
        /// <returns></returns>
        public object Get(object searchCriteriaObject)
        {
            

            DataTable dt = null;
            SqlConnection _con = null;
            string _tableName = "";
            string _primaryKeyName = "";
            PropertyInfosStruct _mappedPrimaryKeyInfos = new PropertyInfosStruct("", "", DataTypes.Integer);
            // List<PropertyInfosStruct> _allPropsInfos = new List<PropertyInfosStruct>();
            //List<PropertyInfosStruct> _mappedColumnsInfos = new List<PropertyInfosStruct>();

            // Obtention du type de l'objet
            Type t = searchCriteriaObject.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;

            
            
            // Found out the Primary key property
            PropertyInfo[] objectProperties = (searchCriteriaObject.GetType()).GetProperties();

            bool hasFoundPrimaryKeyDatas = false;
            foreach (PropertyInfo pInf in objectProperties)
            {

                object[] pkAttr = pInf.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (pkAttr != null && pkAttr.Length > 0)
                {
                    PrimaryKeyAttribute pkA = (PrimaryKeyAttribute)pkAttr[0];
                    _primaryKeyName = pkA.MappedColumnName;
                    object oPKValue = pInf.GetValue(searchCriteriaObject, null);
                    string sPKValue = (oPKValue != null) ? oPKValue.ToString() : null;
                    _mappedPrimaryKeyInfos = new PropertyInfosStruct(pkA.MappedColumnName, sPKValue, DataTypes.Integer);
                    hasFoundPrimaryKeyDatas = true;
                    break;
                }

            }
            if (!hasFoundPrimaryKeyDatas)
                throw new PrimaryKeyException("Couldn't retrieve the primary key informations");




            // Get the Datas from the method GetDatas

            DataTable datas = (DataTable)GetDatas(searchCriteriaObject);

            // Instanciate an object of type of searchCriteriaObject
            System.Reflection.Assembly assembly = t.Assembly;
            
            Object target = assembly.CreateInstance(t.FullName);


          


            // Get the list of all properties
            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] pInfIdCol = pInf.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (pInfIdCol != null && pInfIdCol.Length > 0)
                {
                    PrimaryKeyAttribute pkAttr = (PrimaryKeyAttribute)pInfIdCol[0];
                    pInf.SetValue(target, Convert.ToInt32(datas.Rows[0][pkAttr.MappedColumnName].ToString()), null);
                }
                
                object[] pInfColumns = pInf.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (pInfColumns != null && pInfColumns.Length > 0)
                {
                    ColumnAttribute colAttr = (ColumnAttribute)pInfColumns[0];
                    object data = null;
                    switch( colAttr.DataType)
                    {
                        case DataTypes.Boolean:
                            int valeur = Int32.Parse(datas.Rows[0][colAttr.MappedColumnName].ToString());
                            if (valeur > 0) data = true; else data = false;
                            break;
                        case DataTypes.Decimal:
                            data = Convert.ToDecimal(datas.Rows[0][colAttr.MappedColumnName].ToString());
                            break;
                        case DataTypes.Integer:
                            data = Convert.ToInt32(datas.Rows[0][colAttr.MappedColumnName].ToString());
                            break;
                        case DataTypes.String:
                            data = datas.Rows[0][colAttr.MappedColumnName].ToString();
                            break;


                    }

                    pInf.SetValue(target, data, null);



                }

                


            }



            return target;

        }

        /// <summary>
        /// Obtain the object on a basis of the Id value
        /// Returns the datas as a datatable
        /// </summary>
        /// <param name="searchCriteriaObject"></param>
        /// <returns></returns>
        public object GetDatas(object searchCriteriaObject)
        {
            DataTable dt = null;
            SqlConnection _con = null;
            string _tableName = "";
            string _primaryKeyName = "";
            PropertyInfosStruct _mappedPrimaryKeyInfos = new PropertyInfosStruct("","",DataTypes.Integer);
            //List<PropertyInfosStruct> _mappedColumnsInfos = new List<PropertyInfosStruct>();

            // Obtention du type de l'objet
            Type t = searchCriteriaObject.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;
                       
            PropertyInfo[] objectProperties = (searchCriteriaObject.GetType()).GetProperties();

            bool hasFoundPrimaryKeyDatas = false;
            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] pkAttr = pInf.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (pkAttr != null && pkAttr.Length > 0)
                {
                    PrimaryKeyAttribute pkA = (PrimaryKeyAttribute)pkAttr[0];
                    _primaryKeyName = pkA.MappedColumnName;
                    object oPKValue = pInf.GetValue(searchCriteriaObject, null);
                    string sPKValue = (oPKValue != null) ? oPKValue.ToString() : null;
                    _mappedPrimaryKeyInfos = new PropertyInfosStruct(pkA.MappedColumnName, sPKValue, DataTypes.Integer);
                    hasFoundPrimaryKeyDatas = true;
                    break;
                }

            }
            if (!hasFoundPrimaryKeyDatas)
                throw new PrimaryKeyException("Couldn't retrieve the primary key informations");


            
                  
            // Now we have the connection
            // We have the table name
            // We have the primary key column name, it's value and it's datatype
            // We assume in this simple model that the primary key is always an integer

            StringBuilder request = new StringBuilder();

            request.Append(" SELECT * FROM ")
                .Append(_tableName)
                .Append(" WHERE ")
                .Append(_mappedPrimaryKeyInfos.mappedColumnName)
                .Append("=")
                .Append(_mappedPrimaryKeyInfos.propertyValue);

            if (_con.State == ConnectionState.Closed)
                _con.Open();
            SqlCommand cmd = new SqlCommand(request.ToString(), _con);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet set = new DataSet();
            adapter.Fill(set);

            if (_con.State == ConnectionState.Open)
                _con.Close();

            return set.Tables[0].Copy();
           
        }

        public DataTable GetMany(object searchCriteriaObject, string condition)
        {
            DataTable dt = null;
            SqlConnection _con = null;
            string _tableName = "";
            string _primaryKeyName = "";
            PropertyInfosStruct _mappedPrimaryKeyInfos = new PropertyInfosStruct("", "", DataTypes.Integer);
            //List<PropertyInfosStruct> _mappedColumnsInfos = new List<PropertyInfosStruct>();

            // Obtention du type de l'objet
            Type t = searchCriteriaObject.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;

            PropertyInfo[] objectProperties = (searchCriteriaObject.GetType()).GetProperties();

            bool hasFoundPrimaryKeyDatas = false;
            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] pkAttr = pInf.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (pkAttr != null && pkAttr.Length > 0)
                {
                    PrimaryKeyAttribute pkA = (PrimaryKeyAttribute)pkAttr[0];
                    _primaryKeyName = pkA.MappedColumnName;
                    object oPKValue = pInf.GetValue(searchCriteriaObject, null);
                    string sPKValue = (oPKValue != null) ? oPKValue.ToString() : null;
                    _mappedPrimaryKeyInfos = new PropertyInfosStruct(pkA.MappedColumnName, sPKValue, DataTypes.Integer);
                    hasFoundPrimaryKeyDatas = true;
                    break;
                }

            }
            if (!hasFoundPrimaryKeyDatas)
                throw new PrimaryKeyException("Couldn't retrieve the primary key informations");




            // Now we have the connection
            // We have the table name
            // We have the primary key column name, it's value and it's datatype
            // We assume in this simple model that the primary key is always an integer

            StringBuilder request = new StringBuilder();

            request.Append(" SELECT * FROM ")
                .Append(_tableName)
                .Append(" WHERE ")
                .Append(condition);

            if (_con.State == ConnectionState.Closed)
                _con.Open();
            SqlCommand cmd = new SqlCommand(request.ToString(), _con);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet set = new DataSet();
            adapter.Fill(set);

            if (_con.State == ConnectionState.Open)
                _con.Close();

            return set.Tables[0].Copy();
           
        }

        public DataTable GetAll(object searchCriteriaObject)
        {
            DataTable dt = null;
            SqlConnection _con = null;
            string _tableName = "";
            string _primaryKeyName = "";
            PropertyInfosStruct _mappedPrimaryKeyInfos = new PropertyInfosStruct("", "", DataTypes.Integer);
            //List<PropertyInfosStruct> _mappedColumnsInfos = new List<PropertyInfosStruct>();

            // Obtention du type de l'objet
            Type t = searchCriteriaObject.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;

            PropertyInfo[] objectProperties = (searchCriteriaObject.GetType()).GetProperties();

            bool hasFoundPrimaryKeyDatas = false;
            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] pkAttr = pInf.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (pkAttr != null && pkAttr.Length > 0)
                {
                    PrimaryKeyAttribute pkA = (PrimaryKeyAttribute)pkAttr[0];
                    _primaryKeyName = pkA.MappedColumnName;
                    object oPKValue = pInf.GetValue(searchCriteriaObject, null);
                    string sPKValue = (oPKValue != null) ? oPKValue.ToString() : null;
                    _mappedPrimaryKeyInfos = new PropertyInfosStruct(pkA.MappedColumnName, sPKValue, DataTypes.Integer);
                    hasFoundPrimaryKeyDatas = true;
                    break;
                }

            }
            if (!hasFoundPrimaryKeyDatas)
                throw new PrimaryKeyException("Couldn't retrieve the primary key informations");




            // Now we have the connection
            // We have the table name
            // We have the primary key column name, it's value and it's datatype
            // We assume in this simple model that the primary key is always an integer

            StringBuilder request = new StringBuilder();

            request.Append(" SELECT * FROM ")
                .Append(_tableName);
                


            if (_con.State == ConnectionState.Closed)
                _con.Open();
            SqlCommand cmd = new SqlCommand(request.ToString(), _con);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet set = new DataSet();
            adapter.Fill(set);

            if (_con.State == ConnectionState.Open)
                _con.Close();

            return set.Tables[0].Copy();
           
        }


        public int GetLastIdentity(object searchCriteriaObject)
        {
            
            SqlConnection _con = null;
            string _tableName = "";
            string _primaryKeyName = "";
            PropertyInfosStruct _mappedPrimaryKeyInfos = new PropertyInfosStruct("", "", DataTypes.Integer);
            //List<PropertyInfosStruct> _mappedColumnsInfos = new List<PropertyInfosStruct>();

            // Obtention du type de l'objet
            Type t = searchCriteriaObject.GetType();

            MemberInfo mInfos = t;

            // Gets the connection attribute
            object[] conInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.ConnectionAttribute), true);
            if (conInfos == null)
                throw new UnRegisteredConnectionException("No connection informations found");
            ConnectionAttribute conAttr = (ConnectionAttribute)conInfos[0];

            // Gets the connection object from the Connection Manager
            _con = ConnectionsManager.Get(conAttr.ConnectionID);
            if (_con == null)
                throw new UnRegisteredConnectionException("Failed to initialize connection");

            // Gets the table informations
            object[] tableInfos = mInfos.GetCustomAttributes(typeof(Excelta.NKrusted.Core.TableAttribute), true);
            if (tableInfos == null)
                throw new DataTableNotFoundException("The datatable informations were not found");

            TableAttribute ta = (TableAttribute)tableInfos[0];
            _tableName = ta.MappedTableName;

            PropertyInfo[] objectProperties = (searchCriteriaObject.GetType()).GetProperties();

            bool hasFoundPrimaryKeyDatas = false;
            foreach (PropertyInfo pInf in objectProperties)
            {
                object[] pkAttr = pInf.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (pkAttr != null && pkAttr.Length > 0)
                {
                    PrimaryKeyAttribute pkA = (PrimaryKeyAttribute)pkAttr[0];
                    _primaryKeyName = pkA.MappedColumnName;
                    object oPKValue = pInf.GetValue(searchCriteriaObject, null);
                    string sPKValue = (oPKValue != null) ? oPKValue.ToString() : null;
                    _mappedPrimaryKeyInfos = new PropertyInfosStruct(pkA.MappedColumnName, sPKValue, DataTypes.Integer);
                    hasFoundPrimaryKeyDatas = true;
                    break;
                }

            }
            if (!hasFoundPrimaryKeyDatas)
                throw new PrimaryKeyException("Couldn't retrieve the primary key informations");




            // Now we have the connection
            // We have the table name
            // We have the primary key column name, it's value and it's datatype
            // We assume in this simple model that the primary key is always an integer

            StringBuilder request = new StringBuilder();

            request.Append(" SELECT @@Identity as NewId");
                



            if (_con.State == ConnectionState.Closed)
                _con.Open();
            SqlCommand cmd = new SqlCommand(request.ToString(), _con);

            cmd.CommandType = CommandType.Text;
            int result = Convert.ToInt32( cmd.ExecuteScalar());

            if (_con.State == ConnectionState.Open)
                _con.Close();

            return result;
        }
    }
}
