using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UserModel()
        {
                
        }

        public UserModel(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
}
