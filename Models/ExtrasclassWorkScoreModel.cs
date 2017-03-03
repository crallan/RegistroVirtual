using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ExtrasclassWorkScoreModel
    {
        public int Id { get; set; }
        public int ScoreRegisterId { get; set; }
        public int ExtraclassWorkId { get; set; }
        public float ExtraclassWorkPercentage { get; set; }

        public ExtrasclassWorkScoreModel(int Id, int ScoreRegisterId, int ExtraclassWorkId, float ExtraclassWorkPercentage)
        {
            this.Id = Id;
            this.ScoreRegisterId = ScoreRegisterId;
            this.ExtraclassWorkId = ExtraclassWorkId;
            this.ExtraclassWorkPercentage = ExtraclassWorkPercentage;
        }

        public ExtrasclassWorkScoreModel()
        {

        }
    }
}
