using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excelta.NKrusted.Core;

namespace NKrustedDAO
{ 
[Connection("Server=LOMPO-PC\\SQL2000PE; initial catalog =NKRUSTEDDEMO;user id =sa; password=loverofmysoul","NKRUSTEDDEMO_con")]
[Table("Employees")]
public class C_Employees
{
private string _P_FName;
[Column("FName",DataTypes.String)]
public string P_FName
{
 get { return _P_FName;}
 set { _P_FName = value; }
}


private int _P_Id;
[PrimaryKey("Id")]
public int P_Id
{
 get { return _P_Id;}
 set { _P_Id = value; }
}


private string _P_LName;
[Column("LName",DataTypes.String)]
public string P_LName
{
 get { return _P_LName;}
 set { _P_LName = value; }
}


}
}
