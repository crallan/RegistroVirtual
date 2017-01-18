using Models;
using Services.Model;
using Services.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public bool Authenticate(UserModel user)
        {
            var users = context.Users.Where(x => x.Username.Equals(user.Username)
                       && x.Password.Equals(user.Password));

            if (users.Count() > 0) {
                return true;
            }

            return false;
        }

        public UserModel Get(string id)
        {
            var user = from u in context.Users
                       select new UserModel()
                       {
                           Id = u.Id,
                           Username = u.Username,
                           Password = u.Password
                       };

            return user.FirstOrDefault();
        }

        public bool Save(UserModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
