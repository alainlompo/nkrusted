using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core
{
    public class DataTableNotFoundException:Exception
    {
        public DataTableNotFoundException(string msg)
            : base(msg)
        {
        }
    }
}
