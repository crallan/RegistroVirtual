using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class ExtraclassWorkModel
    {
        public int Id { get; set; }
        public float Percentage { get; set; }
        public int RegisterProfileId { get; set; }

        public ExtraclassWorkModel()
        {

        }

        public ExtraclassWorkModel(float Percentage)
        {
            this.Percentage = Percentage;
        }

    }
}
