using Models;
using Services.Model;
using Services.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public IEnumerable<UserModel> GetList()
        {
            var users = from u in context.Users
                           select new UserModel()
                           {
                               Id = u.Id,
                               FirstName = u.FirstName,
                               LastName = u.LastName
                           };

            return users;
        }

        public UserModel Get(string id)
        {
            var user = from u in context.Users
                       where u.Id.ToString().Equals(id)
                       select new UserModel()
                       {
                           Id = u.Id,
                           Username = u.Username,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Password = u.Password,
                           SelectedSubjects = u.Subjects.Select(s => s.Id).ToList()
                       };

            return user.FirstOrDefault();
        }

        public UserModel GetUserByUsername(string username)
        {
            var user = from u in context.Users
                       where u.Username.Equals(username)
                       select new UserModel()
                       {
                           Id = u.Id,
                           Username = u.Username,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Password = u.Password,
                           SelectedSubjects = u.Subjects.Select(s => s.Id).ToList()
                       };

            return user.FirstOrDefault();
        }

        public bool Save(UserModel user)
        {
            Users dbUser = new Users();
            
            int result;

            try
            {
                //Add
                if (user.Id.Equals(0))
                {
                    dbUser.FirstName = user.FirstName;
                    dbUser.LastName = user.LastName;
                    dbUser.Username = user.Username;
                    dbUser.Password = user.Password;
                    dbUser.Subjects = context.Subjects.Where(x => user.SelectedSubjects.Contains(x.Id)).ToList();

                    context.Users.Add(dbUser);
                    result = context.SaveChanges();
                }
                else
                {
                    // get the record
                    dbUser = context.Users.Single(p => p.Id.Equals(user.Id));

                    // set new values
                    dbUser.FirstName = user.FirstName;
                    dbUser.LastName = user.LastName;
                    dbUser.Username = user.Username;
                    dbUser.Password = user.Password;
                    dbUser.Subjects.Clear();

                    result = context.SaveChanges();

                    dbUser.Subjects = context.Subjects.Where(x => user.SelectedSubjects.Contains(x.Id)).ToList();

                    // save them back to the database
                    result = context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
