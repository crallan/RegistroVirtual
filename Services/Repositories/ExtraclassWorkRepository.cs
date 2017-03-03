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
    public class ExtraclassWorkRepository : IExtraclassWorkRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public ExtraclassWorkModel Get(string id)
        {
            var extraclass = from e in context.ExtraclassWorks
                             where e.Id.ToString().Equals(id)
                          select new ExtraclassWorkModel()
                          {
                              Id = e.Id,
                              Name = e.Name,
                              Percentage = (float)e.Percentage
                          };

            return extraclass.FirstOrDefault();
        }

        public IEnumerable<ExtraclassWorkModel> GetExtraclassWorksByRegisterProfile(int registerId)
        {
            var extraclasses = from e in context.ExtraclassWorks
                               join r in context.RegisterProfiles on e.RegisterProfiles.Id equals r.Id
                               where r.Id.Equals(registerId)
                               orderby e.Id descending
                               select new ExtraclassWorkModel()
                               {
                                   Id = e.Id,
                                   Name = e.Name,
                                   Percentage = (float)e.Percentage
                               };

            return extraclasses;
        }

        public bool Save(ExtraclassWorkModel extraclassModel)
        {
            ExtraclassWorks dbExtraclass = new ExtraclassWorks();

            int result;

            try
            {
                //Add
                if (extraclassModel.Id.Equals(0))
                {
                    dbExtraclass.Name = extraclassModel.Name;
                    dbExtraclass.Percentage = extraclassModel.Percentage;
                    dbExtraclass.RegisterProfiles = context.RegisterProfiles.Single(p => p.Id.Equals(extraclassModel.RegisterProfileId));

                    context.ExtraclassWorks.Add(dbExtraclass);
                    result = context.SaveChanges();
                }
                else
                {
                    // get the record
                    dbExtraclass = context.ExtraclassWorks.Single(p => p.Id.Equals(extraclassModel.Id));

                    // set new values
                    dbExtraclass.Name = extraclassModel.Name;
                    dbExtraclass.Percentage = extraclassModel.Percentage;

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
