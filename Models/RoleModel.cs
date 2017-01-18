using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RoleModel()
        {
        }

        public RoleModel(string Name)
        {
            this.Name = Name;
        }

    }
}
