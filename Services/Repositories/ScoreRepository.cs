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
    public class ScoreRepository : IScoreRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public ScoreModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScoreModel> GetScores(int classId, int year)
        {
            var scores = from s in context.Scores
                         join c in context.Classes on s.Classes.Id equals c.Id
                         where c.Id.Equals(classId) && s.YearCreated.Equals(year)
                         select new ScoreModel()
                         {
                            Id = s.Id,
                            DailyWorkPercentage = (float)s.DailyWorkPercentage,
                            ConceptPercentage = (float)s.ConceptPercentage,
                            AssistancePercentage = (float)s.AssistancePercentage,
                            Absebces = s.Absences,
                            Belated = s.Belated,
                            RegisterProfileId = s.RegisterProfiles.Id,
                            StudentId = s.Students.Id
                         };

            return scores;
        }

        public bool Save(ScoreModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
