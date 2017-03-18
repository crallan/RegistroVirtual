using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ImportModel
    {
        public string FilePath { get; set; }
        public string InstitutionId { get; set; }

        public ImportModel()
        {
        }

        public ImportModel(string FilePath)
        {
            this.FilePath = FilePath;
        }

    }
}
