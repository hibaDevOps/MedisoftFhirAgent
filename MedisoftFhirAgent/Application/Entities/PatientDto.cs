using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Application.Entities
{
    public class PatientDto
    {
		public string patientId { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string middleName { get; set; }
		public string suffix { get; set; }
		public string race { get; set; }
		public string ethnicity { get; set; }
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
		public string sex { get; set; }
		public string dob { get; set; }
		public string defaultPhysician { get; set; }
		public string email { get; set; }
		public string marritalStatus { get; set; }
		public string inactive { get; set; }
		public string chartNumber { get; set; }

	}
}
