using Models;
using Services.Model;
using Services.Specializations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.Repositories
{
    public class ScoreRepository : IScoreRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public ScoreModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScoreModel> GetScores(int classId, int year, int trimester)
        {
            var scores = from s in context.Scores
                         join c in context.Classes on s.Classes.Id equals c.Id
                         where c.Id.Equals(classId) && s.YearCreated.Equals(year) && s.RegisterProfiles.Trimesters.Id.Equals(trimester)
                         select new ScoreModel()
                         {
                            Id = s.Id,
                            DailyWorkPercentage = (float)s.DailyWorkPercentage,
                            ConceptPercentage = (float)s.ConceptPercentage,
                            AssistancePercentage = (float)s.AssistancePercentage,
                            Absebces = s.Absences,
                            Belated = s.Belated,
                            RegisterProfileId = s.RegisterProfiles.Id,
                            StudentId = s.Students.Id,
                            ExamResults = (from ex in context.ExamScores
                                            where ex.Scores.Id.Equals(s.Id)
                                            select new ExamScoreModel() {
                                                Id = ex.Id,
                                                ExamId = ex.Exams.Id,
                                                ScoreRegisterId = ex.Scores.Id,
                                                ExamScore = (float)ex.ExamScore,
                                                ExamPercentage = (float)ex.ExamPercentage
                                            }).ToList(),
                             ExtraclasWorkResults = (from et in context.ExtraclassWorksScores
                                            where et.Scores.Id.Equals(s.Id)
                                            select new ExtrasclassWorkScoreModel()
                                            {
                                                Id = et.Id,
                                                ExtraclassWorkId = et.ExtraclassWorks.Id,
                                                ScoreRegisterId = et.Scores.Id,
                                                ExtraclassWorkPercentage = (float)et.ExtraclassWorkPercentage
                                            }).ToList()
                         };

            return scores;
        }

        public bool Save(ScoreModel entity)
        {
            throw new NotImplementedException();
        }

        public bool SaveScores(List<ScoreModel> scoreList)
        {
            Scores scores = new Scores();

            try
            {
                foreach (ScoreModel scoreEntry in scoreList)
                {
                    //Add
                    if (scoreEntry.Id.Equals(0))
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {

                        }
                    }
                    else
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {

                        }
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }

            return true;
        }
    }
}
