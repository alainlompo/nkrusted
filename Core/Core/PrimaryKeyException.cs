using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core
{
    public class PrimaryKeyException:Exception
    {
        public PrimaryKeyException(string msg):base(msg)
        {

        }
    }
}
