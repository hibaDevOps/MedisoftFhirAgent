using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.DAL.Entities
{
    public class log
    {
        public DateTime LogTime { get; set; }
        public string source { get; set; }
        public string message { get; set; }
    }
}
