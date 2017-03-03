using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ScoreModel
    {
        public int Id { get; set; }
        public int RegisterProfileId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int ClassId { get; set; }
        public float DailyWorkPercentage { get; set; }
        public float ConceptPercentage { get; set; }
        public float AssistancePercentage { get; set; }
        public int Belated { get; set; }
        public int Absebces { get; set; }
        public int YearCreated { get; set; }
        public IEnumerable<ExamScoreModel> ExamResults { get; set; }
        public IEnumerable<ExtrasclassWorkScoreModel> ExtraclasWorkResults { get; set; }

        public ScoreModel()
        {
            ExamResults = new List<ExamScoreModel>();
            ExtraclasWorkResults = new List<ExtrasclassWorkScoreModel>();
        }
    }
}
