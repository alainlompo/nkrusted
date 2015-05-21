using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Excelta.NKrusted.Core
{
    public interface IBatchPersistance
    {
         DataTable GetMany(object searchCriteriaObject, string condition);
         DataTable GetAll(object searchCriteriaObject);
    }
}
