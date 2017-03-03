using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ExamScoreModel
    {
        public int Id { get; set; }
        public int ScoreRegisterId { get; set; }
        public int ExamId { get; set; }
        public float ExamScore { get; set; }
        public float ExamPercentage { get; set; }

        public ExamScoreModel(int Id, int ScoreRegisterId, int ExamId, float ExamScore, float ExamPercentage)
        {
            this.Id = Id;
            this.ScoreRegisterId = ScoreRegisterId;
            this.ExamId = ExamId;
            this.ExamScore = ExamScore;
            this.ExamPercentage = ExamPercentage;
        }

        public ExamScoreModel()
        {

        }
    }
}
