using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Student
    {
        //Methods
        public bool Save(StudentModel student)
        {
            return new StudentRepository().Save(student);
        }

        public bool Import(ImportModel importModel)
        {
            return new StudentRepository().Import(importModel);
        }

        public IEnumerable<StudentModel> GetList()
        {
            return new StudentRepository().GetList();
        }

        public IEnumerable<StudentModel> GetListByClass(int classId)
        {
            return new StudentRepository().GetListByClass(classId);
        }

        public StudentModel Get(string id)
        {
            return new StudentRepository().Get(id);
        }
    }
}
