using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class ClassModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InstitutionId { get; set; }
        public int SchoolYearId { get; set; }
        public int FirstTrimesterProfileId { get; set; }
        public int SecondTrimesterProfileId { get; set; }
        public int ThirdTrimesterProfileId { get; set; }
        public List<SelectListItem> Institutions { get; set; }
        public List<SelectListItem> SchoolYears { get; set; }
        public List<SelectListItem> RegisterProfiles { get; set; }

        public ClassModel()
        {
            Institutions = new List<SelectListItem>();
        }

        public ClassModel(string Name)
        {
            Institutions = new List<SelectListItem>();
            this.Name = Name;
        }

    }
}
