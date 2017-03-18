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
                           RelatedSubjectsAndClasses = (from cu in context.ClassesByUsers
                                                        where cu.UserId.Equals(u.Id)
                                                        select new ClassesBySubjectModel()
                                                        {
                                                            Subject = (from s in context.Subjects
                                                                      where s.Id.Equals(cu.Subjects.Id)
                                                                      select new SubjectModel()
                                                                      {
                                                                          Id = s.Id,
                                                                          Name = s.Name
                                                                      }).FirstOrDefault(),
                                                            SelectedClasses = (from c in context.Classes
                                                                       where c.Id.Equals(cu.Classes.Id)
                                                                       select new ClassModel()
                                                                       {
                                                                           Id = c.Id,
                                                                           Name = c.Name,
                                                                       }).ToList(),
                                                        }).ToList(),
                           InstitutionId = u.Institution != null ? u.Institution.Id : 0
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
                           RelatedSubjectsAndClasses = (from cu in context.ClassesByUsers
                                                        where cu.UserId.Equals(u.Id)
                                                        select new ClassesBySubjectModel()
                                                        {
                                                            Subject = (from s in context.Subjects
                                                                       where s.Id.Equals(cu.Subjects.Id)
                                                                       select new SubjectModel()
                                                                       {
                                                                           Id = s.Id,
                                                                           Name = s.Name
                                                                       }).FirstOrDefault(),
                                                            SelectedClasses = (from c in context.Classes
                                                                               where c.Id.Equals(cu.Classes.Id)
                                                                               select new ClassModel()
                                                                               {
                                                                                   Id = c.Id,
                                                                                   Name = c.Name,
                                                                               }).ToList(),
                                                        }).ToList(),
                           InstitutionId = u.Institution != null ? u.Institution.Id : 0
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
                    dbUser.Institution = context.Institution.Single(p => p.Id.Equals(user.InstitutionId));
                    
                    context.Users.Add(dbUser);
                    result = context.SaveChanges();

                    foreach (ClassesBySubjectModel relatedSubject in user.RelatedSubjectsAndClasses)
                    {
                        foreach (ClassModel relatedClass in relatedSubject.SelectedClasses)
                        {
                            ClassesByUsers newClassByUser = new ClassesByUsers();
                            newClassByUser.Subjects = context.Subjects.Single(p => p.Id.Equals(relatedSubject.Subject.Id));
                            newClassByUser.Classes = context.Classes.Single(p => p.Id.Equals(relatedClass.Id));
                            newClassByUser.Users = dbUser;
                            newClassByUser.YearCreated = DateTime.Now.Year;

                            context.ClassesByUsers.Add(newClassByUser);
                        }
                    }

                    // save them back to the database
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
                    dbUser.Institution = context.Institution.Single(p => p.Id.Equals(user.InstitutionId));
                    dbUser.ClassesByUsers.Clear();

                    result = context.SaveChanges();

                    foreach (ClassesBySubjectModel relatedSubject in user.RelatedSubjectsAndClasses)
                    {
                        foreach (ClassModel relatedClass in relatedSubject.SelectedClasses)
                        {
                            ClassesByUsers newClassByUser = new ClassesByUsers();
                            newClassByUser.Subjects = context.Subjects.Single(p => p.Id.Equals(relatedSubject.Subject.Id));
                            newClassByUser.Classes = context.Classes.Single(p => p.Id.Equals(relatedClass.Id));
                            newClassByUser.Users = dbUser;
                            newClassByUser.YearCreated = DateTime.Now.Year;

                            context.ClassesByUsers.Add(newClassByUser);
                        }
                    }

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
