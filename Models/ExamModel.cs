using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class ExamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Percentage { get; set; }
        public int Score { get; set; }
        public int RegisterProfileId { get; set; }

        public ExamModel()
        {

        }

        public ExamModel(string Name, float Percentage, int Score)
        {
            this.Name = Name;
            this.Percentage = Percentage;
            this.Score = Score;
        }

    }
}
