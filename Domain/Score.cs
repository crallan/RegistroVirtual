using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Score
    {
        //Methods
        public IEnumerable<ScoreModel> GetScores(int classId, int year, int trimester, int subject)
        {
            return new ScoreRepository().GetScores(classId, year, trimester, subject);
        }

        public IEnumerable<ScoreModel> GetScores(int classId, int year, int subject)
        {
            return new ScoreRepository().GetScores(classId, year, subject);
        }

        public ScoreModel GetStudentScore(int classId, int year, int trimester, int subject, int studentId)
        {
            return new ScoreRepository().GetStudentScore(classId, year, trimester, subject, studentId);
        }

        public bool Save(List<ScoreModel> scores)
        {
            return new ScoreRepository().SaveScores(scores);
        }
    }
}
