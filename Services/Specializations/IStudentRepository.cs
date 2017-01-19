using Models;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specializations
{
    public interface IStudentRepository : IRepository<StudentModel, string>
    {
        IEnumerable<StudentModel> GetList();
        IEnumerable<StudentModel> GetListByClass(int classId);
        bool Import(ImportModel importModel);
    }
}
