using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return this.FirstName + " " + this.LastName; } }
        public List<ClassesBySubjectModel> RelatedSubjectsAndClasses { get; set; }
        public int InstitutionId { get; set; }
        public List<SelectListItem> Institutions { get; set; }

        public UserModel()
        {
            this.RelatedSubjectsAndClasses = new List<ClassesBySubjectModel>();
        }

        public UserModel(string Username, string Password, string FirstName, string LastName)
        {
            this.RelatedSubjectsAndClasses = new List<ClassesBySubjectModel>();
            this.Username = Username;
            this.Password = Password;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
    }
}
