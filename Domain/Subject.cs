using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Subject
    {
        public IEnumerable<SubjectModel> GetList()
        {
            return new SubjectRepository().GetList();
        }
    }
}
