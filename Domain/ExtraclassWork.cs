using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ExtraclassWork
    {
        //Methods
        public IEnumerable<ExtraclassWorkModel> GetExtraclassWorksByRegisterProfile(int registerId)
        {
            return new ExtraclassWorkRepository().GetExtraclassWorksByRegisterProfile(registerId);
        }

        public bool Save(ExtraclassWorkModel extraclass)
        {
            return new ExtraclassWorkRepository().Save(extraclass);
        }

        public ExtraclassWorkModel Get(string id)
        {
            return new ExtraclassWorkRepository().Get(id);
        }
    }
}
