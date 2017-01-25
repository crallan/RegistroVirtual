using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SchoolYearModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }

        public SchoolYearModel()
        {
        }

        public SchoolYearModel(string Name, int Year)
        {
            this.Name = Name;
            this.Year = Year;
        }

    }
}
