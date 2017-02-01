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
    public class ExamRepository : IExamRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public ExamModel Get(string id)
        {
            var exam = from e in context.Exams
                          where e.Id.ToString().Equals(id)
                          select new ExamModel()
                          {
                              Id = e.Id,
                              Percentage = (float)e.Percentage,
                              Score = e.Score
                          };

            return exam.FirstOrDefault();
        }

        public IEnumerable<ExamModel> GetExamsByRegisterProfile(int registerId)
        {
            var exams = from e in context.Exams
                        join r in context.RegisterProfiles on e.RegisterProfiles.Id equals r.Id
                        where r.Id.Equals(registerId)
                        select new ExamModel()
                       {
                           Id = e.Id,
                           Percentage = (float)e.Percentage,
                           Score = e.Score
                       };

            return exams;
        }

        public bool Save(ExamModel examModel)
        {
            Exams dbExam = new Exams();

            int result;

            try
            {
                //Add
                if (examModel.Id.Equals(0))
                {
                    dbExam.Percentage = examModel.Percentage;
                    dbExam.Score = examModel.Score;
                    dbExam.RegisterProfiles = context.RegisterProfiles.Single(p => p.Id.Equals(examModel.RegisterProfileId));

                    context.Exams.Add(dbExam);
                    result = context.SaveChanges();
                }
                else
                {
                    // get the record
                    dbExam = context.Exams.Single(p => p.Id.Equals(examModel.Id));

                    // set new values
                    dbExam.Percentage = examModel.Percentage;
                    dbExam.Score = examModel.Score;

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
