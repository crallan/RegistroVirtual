using Models;
using Services.Model;
using Services.Specializations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class ClassRepository : IClassRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public ClassModel Get(string id)
        {
            var @class = from c in context.Classes
                         where c.Id.ToString().Equals(id)
                         select new ClassModel()
                         {
                             Id = c.Id,
                             Name = c.Name,
                             SchoolYearId = c.SchoolYears.Id,
                             FirstTrimesterProfileId = c.RegisterProfiles != null ? c.RegisterProfiles.Id : 0,
                             SecondTrimesterProfileId = c.RegisterProfiles1 != null ? c.RegisterProfiles1.Id : 0,
                             ThirdTrimesterProfileId = c.RegisterProfiles2 != null ? c.RegisterProfiles2.Id : 0,
                             InstitutionId = c.Institution != null ? c.Institution.Id : 0
                         };

            return @class.FirstOrDefault();
        }

        public IEnumerable<ClassModel> GetClassesList()
        {
            var classes = from c in context.Classes
                       select new ClassModel()
                       {
                           Id = c.Id,
                           Name = c.Name,
                           SchoolYearId = c.SchoolYears.Id,
                           FirstTrimesterProfileId = c.RegisterProfiles != null ? c.RegisterProfiles.Id : 0,
                           SecondTrimesterProfileId = c.RegisterProfiles1 != null ? c.RegisterProfiles1.Id : 0,
                           ThirdTrimesterProfileId = c.RegisterProfiles2 != null ? c.RegisterProfiles2.Id : 0,
                           InstitutionId = c.Institution != null ? c.Institution.Id : 0
                       };

            return classes;
        }

        public IEnumerable<ClassModel> GetClassesListByInstitution(int institutionId)
        {
            var classes = from c in context.Classes
                          where c.Institution != null && c.Institution.Id.Equals(institutionId)
                          select new ClassModel()
                          {
                              Id = c.Id,
                              Name = c.Name,
                              SchoolYearId = c.SchoolYears.Id,
                              FirstTrimesterProfileId = c.RegisterProfiles != null ? c.RegisterProfiles.Id : 0,
                              SecondTrimesterProfileId = c.RegisterProfiles1 != null ? c.RegisterProfiles1.Id : 0,
                              ThirdTrimesterProfileId = c.RegisterProfiles2 != null ? c.RegisterProfiles2.Id : 0,
                              InstitutionId = c.Institution != null ? c.Institution.Id : 0
                          };

            return classes;
        }

        public IEnumerable<ClassModel> GetClassesListByUser(int userId)
        {
            var classes = from c in context.Classes
                          where c.Institution != null && c.Institution.Id.Equals(userId)
                          select new ClassModel()
                          {
                              Id = c.Id,
                              Name = c.Name,
                              SchoolYearId = c.SchoolYears.Id,
                              FirstTrimesterProfileId = c.RegisterProfiles != null ? c.RegisterProfiles.Id : 0,
                              SecondTrimesterProfileId = c.RegisterProfiles1 != null ? c.RegisterProfiles1.Id : 0,
                              ThirdTrimesterProfileId = c.RegisterProfiles2 != null ? c.RegisterProfiles2.Id : 0,
                              InstitutionId = c.Institution != null ? c.Institution.Id : 0
                          };

            return classes;
        }

        public bool Save(ClassModel classModel)
        {
            Classes dbClass = new Classes();

            int result;

            try
            {
                //Add
                if (classModel.Id.Equals(0))
                {
                    dbClass.Name = classModel.Name;
                    dbClass.Institution = context.Institution.Single(p => p.Id.Equals(classModel.InstitutionId));
                    dbClass.SchoolYears = context.SchoolYears.Single(p => p.Id.Equals(classModel.SchoolYearId));
                    dbClass.YearCreated = DateTime.Now.Year;
                    dbClass.RegisterProfiles = context.RegisterProfiles.Single(p => p.Id.Equals(classModel.FirstTrimesterProfileId));
                    dbClass.RegisterProfiles1 = context.RegisterProfiles.Single(p => p.Id.Equals(classModel.SecondTrimesterProfileId));
                    dbClass.RegisterProfiles2 = context.RegisterProfiles.Single(p => p.Id.Equals(classModel.ThirdTrimesterProfileId));

                    context.Classes.Add(dbClass);
                    result = context.SaveChanges();
                }
                else
                {
                    // get the record
                    dbClass = context.Classes.Single(p => p.Id.Equals(classModel.Id));

                    // set new values
                    dbClass.Name = classModel.Name;
                    dbClass.Institution = context.Institution.Single(p => p.Id.Equals(classModel.InstitutionId));
                    dbClass.SchoolYears = context.SchoolYears.Single(p => p.Id.Equals(classModel.SchoolYearId));
                    dbClass.RegisterProfiles = context.RegisterProfiles.Single(p => p.Id == classModel.FirstTrimesterProfileId);
                    dbClass.RegisterProfiles1 = context.RegisterProfiles.Single(p => p.Id == classModel.SecondTrimesterProfileId);
                    dbClass.RegisterProfiles2 = context.RegisterProfiles.Single(p => p.Id == classModel.ThirdTrimesterProfileId);

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
