using Models;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specializations
{
    public interface IScoreRepository : IRepository<ScoreModel, string>
    {
        IEnumerable<ScoreModel> GetScores(int classId, int year, int trimester, int subject);
        bool SaveScores(List<ScoreModel> scores);
    }
}
