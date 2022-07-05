using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Application.Entities
{
    public class PatientAddressDto
    {
        public string streetName { set; get; }
        public string streetNo { set; get; }
        public string appartmentNo { set; get; }
        public string postalCode { set; get; }
        public string city { set; get; }
        public string country { set; get; }
        public string type { set; get; }
    }
}
