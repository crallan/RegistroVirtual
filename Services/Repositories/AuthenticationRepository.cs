using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public bool Authenticate(UserModel user)
        {
            if (user.Username.Equals("achacon") && user.Passsword.Equals("123")) {
                return true;
            }

            return false;
        }

        public UserModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Save(UserModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
