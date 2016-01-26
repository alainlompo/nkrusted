using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core
{
	/* This discrimantes exceptions related to unregistered connections */
    public class UnRegisteredConnectionException:Exception
    {
        public UnRegisteredConnectionException(string msg):base(msg)
        {


        }
    }
}
