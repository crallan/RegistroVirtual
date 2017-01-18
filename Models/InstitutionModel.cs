using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InstitutionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public InstitutionModel()
        {
        }

        public InstitutionModel(string Name)
        {
            this.Name = Name;
        }

    }
}
