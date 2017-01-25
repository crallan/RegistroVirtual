using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TrimesterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TrimesterModel()
        {
        }

        public TrimesterModel(string Name)
        {
            this.Name = Name;
        }

    }
}
