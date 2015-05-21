using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelta.NKrusted.Core
{
    public interface IPersistanceController
    {
         int Add(object datasObject);
         int Update(object datasHolder);
         int Delete(object objectToDelete);
         object Get(object searchCriteriaObject);

    }
}
