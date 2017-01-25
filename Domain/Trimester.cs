using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Trimester
    {
        //Methods
        public IEnumerable<TrimesterModel> GetTrimesters()
        {
            return new TrimesterRepository().GetTrimesters();
        }

    }
}
