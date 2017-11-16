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

        public IEnumerable<ScoreModel> GetScores(int classId, int year, int trimester, int subject)
        {
            var scores = from s in context.Scores
                         join c in context.Classes on s.Classes.Id equals c.Id
                         where c.Id.Equals(classId) 
                                && s.YearCreated.Equals(year) 
                                && s.RegisterProfiles.Trimesters.Id.Equals(trimester) 
                                && s.RegisterProfiles.Subjects.Id.Equals(subject)
                         select new ScoreModel()
                         {
                            Id = s.Id,
                            DailyWorkPercentage = (float)s.DailyWorkPercentage,
                            DailyWorkScore = (float)s.DailyWorkScore,
                            ConceptPercentage = (float)s.ConceptPercentage,
                            AssistancePercentage = (float)s.AssistancePercentage,
                            Absences = s.Absences,
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
                                                ExamPoints = (float)ex.ExamPoints,
                                                ExamPercentage = (float)ex.ExamPercentage
                                            }).ToList(),
                             ExtraclasWorkResults = (from et in context.ExtraclassWorksScores
                                            where et.Scores.Id.Equals(s.Id)
                                            select new ExtrasclassWorkScoreModel()
                                            {
                                                Id = et.Id,
                                                ExtraclassWorkId = et.ExtraclassWorks.Id,
                                                ScoreRegisterId = et.Scores.Id,
                                                ExtraclassWorkPercentage = (float)et.ExtraclassWorkPercentage,
                                                ExtraclassWorkScore = (float)et.ExtraclassWorkScore
                                            }).ToList()
                         };

            return scores;
        }

        public IEnumerable<ScoreModel> GetScores(int classId, int year, int subject)
        {
            var scores = from s in context.Scores
                         join c in context.Classes on s.Classes.Id equals c.Id
                         where c.Id.Equals(classId)
                                && s.YearCreated.Equals(year)
                                && s.RegisterProfiles.Subjects.Id.Equals(subject)
                         select new ScoreModel()
                         {
                             Id = s.Id,
                             DailyWorkPercentage = (float)s.DailyWorkPercentage,
                             DailyWorkScore = (float)s.DailyWorkScore,
                             ConceptPercentage = (float)s.ConceptPercentage,
                             AssistancePercentage = (float)s.AssistancePercentage,
                             Absences = s.Absences,
                             Belated = s.Belated,
                             RegisterProfileId = s.RegisterProfiles.Id,
                             StudentId = s.Students.Id,
                             ExamResults = (from ex in context.ExamScores
                                            where ex.Scores.Id.Equals(s.Id)
                                            select new ExamScoreModel()
                                            {
                                                Id = ex.Id,
                                                ExamId = ex.Exams.Id,
                                                ScoreRegisterId = ex.Scores.Id,
                                                ExamScore = (float)ex.ExamScore,
                                                ExamPoints = (float)ex.ExamPoints,
                                                ExamPercentage = (float)ex.ExamPercentage
                                            }).ToList(),
                             ExtraclasWorkResults = (from et in context.ExtraclassWorksScores
                                                     where et.Scores.Id.Equals(s.Id)
                                                     select new ExtrasclassWorkScoreModel()
                                                     {
                                                         Id = et.Id,
                                                         ExtraclassWorkId = et.ExtraclassWorks.Id,
                                                         ScoreRegisterId = et.Scores.Id,
                                                         ExtraclassWorkPercentage = (float)et.ExtraclassWorkPercentage,
                                                         ExtraclassWorkScore = (float)et.ExtraclassWorkScore
                                                     }).ToList()
                         };

            return scores;
        }

        public ScoreModel GetStudentScore(int classId, int year, int trimester, int subject,int student)
        {
            var score = from s in context.Scores
                         join c in context.Classes on s.Classes.Id equals c.Id
                         where c.Id.Equals(classId)
                                && s.YearCreated.Equals(year)
                                && s.RegisterProfiles.Trimesters.Id.Equals(trimester)
                                && s.RegisterProfiles.Subjects.Id.Equals(subject)
                                && s.Students.Id.Equals(student)
                         select new ScoreModel()
                         {
                             Id = s.Id,
                             DailyWorkPercentage = (float)s.DailyWorkPercentage,
                             DailyWorkScore = (float)s.DailyWorkScore,
                             ConceptPercentage = (float)s.ConceptPercentage,
                             AssistancePercentage = (float)s.AssistancePercentage,
                             Absences = s.Absences,
                             Belated = s.Belated,
                             RegisterProfileId = s.RegisterProfiles.Id,
                             StudentId = s.Students.Id,
                             ExamResults = (from ex in context.ExamScores
                                            where ex.Scores.Id.Equals(s.Id)
                                            select new ExamScoreModel()
                                            {
                                                Id = ex.Id,
                                                ExamId = ex.Exams.Id,
                                                ScoreRegisterId = ex.Scores.Id,
                                                ExamScore = (float)ex.ExamScore,
                                                ExamPoints = (float)ex.ExamPoints,
                                                ExamPercentage = (float)ex.ExamPercentage
                                            }).ToList(),
                             ExtraclasWorkResults = (from et in context.ExtraclassWorksScores
                                                     where et.Scores.Id.Equals(s.Id)
                                                     select new ExtrasclassWorkScoreModel()
                                                     {
                                                         Id = et.Id,
                                                         ExtraclassWorkId = et.ExtraclassWorks.Id,
                                                         ScoreRegisterId = et.Scores.Id,
                                                         ExtraclassWorkPercentage = (float)et.ExtraclassWorkPercentage,
                                                         ExtraclassWorkScore = (float)et.ExtraclassWorkScore
                                                     }).ToList()
                         };

            return score.FirstOrDefault();
        }

        public bool Save(ScoreModel entity)
        {
            throw new NotImplementedException();
        }

        public bool SaveScores(List<ScoreModel> scoreList)
        {
            int result;

            try
            {
                foreach (ScoreModel scoreEntry in scoreList)
                {
                    Scores dbScores = new Scores();

                    //Add
                    if (scoreEntry.Id.Equals(0))
                    {
                        dbScores.RegisterProfiles = context.RegisterProfiles.Single(p => p.Id.Equals(scoreEntry.RegisterProfileId));
                        dbScores.Classes = context.Classes.Single(p => p.Id.Equals(scoreEntry.ClassId));
                        dbScores.Students = context.Students.Single(p => p.Id.Equals(scoreEntry.StudentId));
                        dbScores.Absences = scoreEntry.Absences;
                        dbScores.Belated = scoreEntry.Belated;
                        dbScores.AssistancePercentage = scoreEntry.AssistancePercentage;
                        dbScores.DailyWorkPercentage = scoreEntry.DailyWorkPercentage;
                        dbScores.DailyWorkScore = scoreEntry.DailyWorkScore;
                        dbScores.ConceptPercentage = scoreEntry.ConceptPercentage;
                        dbScores.YearCreated = scoreEntry.YearCreated;

                        context.Scores.Add(dbScores);
                        result = context.SaveChanges();

                        int scoreId = dbScores.Id;

                        if (!scoreId.Equals(0))
                        {
                            foreach (ExamScoreModel examModel in scoreEntry.ExamResults)
                            {
                                ExamScores exam = new ExamScores();
                                exam.Scores = dbScores;
                                exam.ExamScore = examModel.ExamScore;
                                exam.ExamPoints = examModel.ExamPoints;
                                exam.ExamPercentage = examModel.ExamPercentage;
                                exam.Exams = context.Exams.Single(p => p.Id.Equals(examModel.ExamId));

                                context.ExamScores.Add(exam);
                            }

                            foreach (ExtrasclassWorkScoreModel extraclassModel in scoreEntry.ExtraclasWorkResults)
                            {
                                ExtraclassWorksScores extraclass = new ExtraclassWorksScores();
                                extraclass.Scores = dbScores;
                                extraclass.ExtraclassWorks = context.ExtraclassWorks.Single(p => p.Id.Equals(extraclassModel.ExtraclassWorkId));
                                extraclass.ExtraclassWorkPercentage = extraclassModel.ExtraclassWorkPercentage;
                                extraclass.ExtraclassWorkScore = extraclassModel.ExtraclassWorkScore;

                                context.ExtraclassWorksScores.Add(extraclass);
                            }

                            result = context.SaveChanges();
                        }

                    }
                    else
                    {
                        // get the record
                        dbScores = context.Scores.Single(p => p.Id.Equals(scoreEntry.Id));

                        // set new values
                        dbScores.Absences = scoreEntry.Absences;
                        dbScores.Belated = scoreEntry.Belated;
                        dbScores.AssistancePercentage = scoreEntry.AssistancePercentage;
                        dbScores.DailyWorkPercentage = scoreEntry.DailyWorkPercentage;
                        dbScores.DailyWorkScore = scoreEntry.DailyWorkScore;
                        dbScores.ConceptPercentage = scoreEntry.ConceptPercentage;

                        foreach (ExamScoreModel examModel in scoreEntry.ExamResults)
                        {
                            ExamScores exam = dbScores.ExamScores.FirstOrDefault(p => p.Exams.Id.Equals(examModel.ExamId));

                            if (exam != null)
                            {
                                exam.ExamScore = examModel.ExamScore;
                                exam.ExamPoints = examModel.ExamPoints;
                                exam.ExamPercentage = examModel.ExamPercentage;
                            }
                            else
                            {
                                exam = new ExamScores();
                                exam.Scores = dbScores;
                                exam.ExamScore = examModel.ExamScore;
                                exam.ExamPoints = examModel.ExamPoints;
                                exam.Exams = context.Exams.Single(p => p.Id.Equals(examModel.ExamId));
                                exam.ExamPercentage = examModel.ExamPercentage;

                                context.ExamScores.Add(exam);
                            }
                        }

                        foreach (ExtrasclassWorkScoreModel extraclassModel in scoreEntry.ExtraclasWorkResults)
                        {

                            ExtraclassWorksScores extraclass = dbScores.ExtraclassWorksScores.FirstOrDefault(p => p.ExtraclassWorks.Id.Equals(extraclassModel.ExtraclassWorkId));

                            if (extraclass != null)
                            {
                                extraclass.ExtraclassWorkPercentage = extraclassModel.ExtraclassWorkPercentage;
                                extraclass.ExtraclassWorkScore = extraclassModel.ExtraclassWorkScore;
                            }
                            else
                            {
                                extraclass = new ExtraclassWorksScores();
                                extraclass.Scores = dbScores;
                                extraclass.ExtraclassWorks = context.ExtraclassWorks.Single(p => p.Id.Equals(extraclassModel.ExtraclassWorkId));
                                extraclass.ExtraclassWorkPercentage = extraclassModel.ExtraclassWorkPercentage;
                                extraclass.ExtraclassWorkScore = extraclassModel.ExtraclassWorkScore;

                                context.ExtraclassWorksScores.Add(extraclass);
                            }
                        }

                        // save them back to the database
                        result = context.SaveChanges();
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
