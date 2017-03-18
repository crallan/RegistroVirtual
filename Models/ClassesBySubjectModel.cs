using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ClassesBySubjectModel
    {
        public SubjectModel Subject { get; set; }
        public List<ClassModel> SelectedClasses { get; set; }

        public ClassesBySubjectModel()
        {
            this.Subject = new SubjectModel();
            this.SelectedClasses = new List<ClassModel>();
        }
    }
}
