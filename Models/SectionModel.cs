using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SectionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SectionModel()
        {
        }

        public SectionModel(string Name)
        {
            this.Name = Name;
        }

    }
}
