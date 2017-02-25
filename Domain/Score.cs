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
        public IEnumerable<ScoreModel> GetScores(int classId, int year)
        {
            return new ScoreRepository().GetScores(classId, year);
        }
    }
}
