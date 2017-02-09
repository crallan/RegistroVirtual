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
    public class SubjectRepository : ISubjectRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public SubjectModel Get(string id)
        {
            var subject = from s in context.Subjects
                          where s.Id.Equals(id)
                          select new SubjectModel()
                           {
                               Id = s.Id,
                               Name = s.Name
                           };

            return subject.FirstOrDefault();
        }

        public IEnumerable<SubjectModel> GetList()
        {
            var subjects = from s in context.Subjects
                        select new SubjectModel()
                        {
                            Id = s.Id,
                            Name = s.Name
                        };

            return subjects;
        }

        public bool Save(SubjectModel entity)
        {
            throw new NotImplementedException();
        }
    }
    
}
