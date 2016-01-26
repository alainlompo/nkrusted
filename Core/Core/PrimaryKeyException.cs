using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core
{
	/* This Exception is meant to discriminate exceptions related to primary key */
    public class PrimaryKeyException:Exception
    {
        public PrimaryKeyException(string msg):base(msg)
        {

        }
    }
}
