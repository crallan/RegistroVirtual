using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Institution
    {
        //Methods
        public IEnumerable<InstitutionModel> GetInstitutionsList()
        {
            return new InstitutionRepository().GetInstitutionsList();
        }

        public bool Save(InstitutionModel institution)
        {
            return new InstitutionRepository().Save(institution);
        }

        public InstitutionModel Get(string id)
        {
            return new InstitutionRepository().Get(id);
        }
    }
}
