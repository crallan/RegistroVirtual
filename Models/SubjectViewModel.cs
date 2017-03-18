using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MultiSelectList Classes { get; set; }
        public List<int> SelectedClasses { get; set; }

        public SubjectViewModel()
        {
        }

        public SubjectViewModel(string Name)
        {
            this.Name = Name;
        }

    }
}
