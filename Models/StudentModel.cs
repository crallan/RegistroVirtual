using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public StudentModel()
        {
        }

        public StudentModel(string FirstName, string LastName, int ClassId)
        {
            this.ClassId = ClassId;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
    }
}
