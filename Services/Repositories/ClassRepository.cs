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
            throw new NotImplementedException();
        }

        public IEnumerable<ClassModel> GetClassesList()
        {
            var classes = from c in context.Classes
                       select new ClassModel()
                       {
                           Id = c.Id,
                           Name = c.Name
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
                if (dbClass.Id.Equals(0))
                {
                    dbClass.Name = classModel.Name;

                    context.Classes.Add(dbClass);
                    result = context.SaveChanges();
                }
                else
                {
                    // get the record
                    dbClass = context.Classes.Single(p => p.Id.Equals(classModel.Id));

                    // set new values
                    dbClass.Name = classModel.Name;

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
