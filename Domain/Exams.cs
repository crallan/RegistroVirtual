using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Exams
    {
        //Methods
        public IEnumerable<ExamModel> GetExamsByRegisterProfile(int registerId)
        {
            return new ExamRepository().GetExamsByRegisterProfile(registerId);
        }

        public bool Save(ExamModel exam)
        {
            return new ExamRepository().Save(exam);
        }

        public ExamModel Get(string id)
        {
            return new ExamRepository().Get(id);
        }
    }
}
