using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Class
    {
        //Methods
        public IEnumerable<ClassModel> GetClassesList()
        {
            return new ClassRepository().GetClassesList();
        }

        public bool Save(ClassModel @class)
        {
            return new ClassRepository().Save(@class);
        }

        public ClassModel Get(string id)
        {
            return new ClassRepository().Get(id);
        }
    }
}
