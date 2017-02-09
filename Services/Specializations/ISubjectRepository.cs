using Models;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specializations
{
    public interface ISubjectRepository : IRepository<SubjectModel, string>
    {
        IEnumerable<SubjectModel> GetList();
    }
}
