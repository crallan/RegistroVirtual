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
    public class InstitutionRepository : IInstitutionRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public InstitutionModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InstitutionModel> GetInstitutionsList()
        {
            var institutions = from i in context.Institution
                       select new InstitutionModel()
                       {
                           Id = i.Id,
                           Name = i.Name
                       };

            return institutions;
        }

        public bool Save(InstitutionModel institutionModel)
        {
            Institution dbInstitution = new Institution();

            int result;

            try
            {
                //Add
                if (dbInstitution.Id.Equals(0))
                {
                    dbInstitution.Name = institutionModel.Name;

                    context.Institution.Add(dbInstitution);
                    result = context.SaveChanges();
                }
                else
                {
                    // get the record
                    dbInstitution = context.Institution.Single(p => p.Id.Equals(institutionModel.Id));

                    // set new values
                    dbInstitution.Name = institutionModel.Name;

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
