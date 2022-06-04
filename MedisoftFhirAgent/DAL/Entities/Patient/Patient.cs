using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.DAL.Entities
{
    public class Patient
    {
        public string Identifier { get; set; }
        public string prefix { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string birthDate { get; set; }
        public string birthPlace { get; set; }
        public string citizenshipCode { get; set; }
        public string gender { get; set; }
        public PatientAddress address { get; set; }
     
    }

}
