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
		public string language { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string zip { get; set; }
		public string wPhone { get; set; }

		public string hPhone { get; set; }
		public string mobile { get; set; }
		public string fax { get; set; }
		public string altPhone { get; set; }
		public string ssn { get; set; }
		public string defaultPhysician { get; set; }
		public string email { get; set; }
		public string marritalStatus { get; set; }
		public string inactive { get; set; }


	}

}
