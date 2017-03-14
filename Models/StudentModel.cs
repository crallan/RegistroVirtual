using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string CardId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SelectListItem> Classes { get; set; }

        public StudentModel()
        {
            Classes = new List<SelectListItem>();
        }

        public StudentModel(string FirstName, string LastName, int ClassId)
        {
            Classes = new List<SelectListItem>();

            this.ClassId = ClassId;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
    }
}
