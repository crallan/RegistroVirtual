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
    }
}
