using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SchoolYear
    {
        //Methods
        public IEnumerable<SchoolYearModel> GetSchoolYears()
        {
            return new SchoolYearRepository().GetSchoolYears();
        }

    }
}
