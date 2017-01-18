using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SubjectModel()
        {
        }

        public SubjectModel(string Name)
        {
            this.Name = Name;
        }

    }
}
