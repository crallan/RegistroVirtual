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
    public class TrimesterRepository : ITrimesterRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public IEnumerable<TrimesterModel> GetTrimesters()
        {
            var trimesters = from t in context.Trimesters
                          select new TrimesterModel()
                          {
                              Id = t.Id,
                              Name = t.Name
                          };

            return trimesters;
        }

        public bool Save(TrimesterModel entity)
        {
            throw new NotImplementedException();
        }

        TrimesterModel IRepository<TrimesterModel, string>.Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
