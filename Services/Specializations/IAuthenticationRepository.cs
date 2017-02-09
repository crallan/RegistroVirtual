using Models;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specializations
{
    public interface IAuthenticationRepository : IRepository<UserModel, string>
    {
        bool Authenticate(UserModel user);
        UserModel GetUserByUsername(string username);
        IEnumerable<UserModel> GetList();
    }
}
