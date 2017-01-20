using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        //Methods
        public bool Authenticate(UserModel user) {
            return new AuthenticationRepository().Authenticate(user);
        }

        public UserModel GetUserByUsername(string userName)
        {
            return new AuthenticationRepository().GetUserByUsername(userName);
        }

    }
}
