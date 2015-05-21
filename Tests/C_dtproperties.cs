using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excelta.NKrusted.Core;

namespace NKrustedDAO
{ 
[Connection("Server=LOMPO-PC\\SQL2000PE; initial catalog =NKRUSTEDDEMO;user id =sa; password=loverofmysoul","NKRUSTEDDEMO_con")]
[Table("dtproperties")]
public class C_dtproperties
{
private int _P_id;
[PrimaryKey("id")]
public int P_id
{
 get { return _P_id;}
 set { _P_id = value; }
}


private string _P_lvalue;
[Column("lvalue",DataTypes.String)]
public string P_lvalue
{
 get { return _P_lvalue;}
 set { _P_lvalue = value; }
}


private int _P_objectid;
[Column("objectid",DataTypes.Integer)]
public int P_objectid
{
 get { return _P_objectid;}
 set { _P_objectid = value; }
}


private string _P_property;
[PrimaryKey("property")]
public string P_property
{
 get { return _P_property;}
 set { _P_property = value; }
}


private string _P_uvalue;
[Column("uvalue",DataTypes.String)]
public string P_uvalue
{
 get { return _P_uvalue;}
 set { _P_uvalue = value; }
}


private string _P_value;
[Column("value",DataTypes.String)]
public string P_value
{
 get { return _P_value;}
 set { _P_value = value; }
}


private int _P_version;
[Column("version",DataTypes.Integer)]
public int P_version
{
 get { return _P_version;}
 set { _P_version = value; }
}


}
}
