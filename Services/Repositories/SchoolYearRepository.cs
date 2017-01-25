using Models;
using Services.Model;
using Services.Specializations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Contract;

namespace Services.Repositories
{
    public class SchoolYearRepository : ISchoolYearRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public IEnumerable<SchoolYearModel> GetSchoolYears()
        {
            var schoolYears = from s in context.SchoolYears
                          select new SchoolYearModel()
                          {
                              Id = s.Id,
                              Name = s.Name,
                              Year = s.Year
                          };

            return schoolYears;
        }

        public bool Save(SchoolYearModel entity)
        {
            throw new NotImplementedException();
        }

        SchoolYearModel IRepository<SchoolYearModel, string>.Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
