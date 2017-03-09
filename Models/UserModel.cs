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
        public List<int> SelectedSubjects { get; set; }
        public MultiSelectList Subjects { get; set; }

        public UserModel()
        {
        }

        public UserModel(string Username, string Password, string FirstName, string LastName)
        {
            this.Username = Username;
            this.Password = Password;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
    }
}
