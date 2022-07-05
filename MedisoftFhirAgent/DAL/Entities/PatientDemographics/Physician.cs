using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.DAL.Entities
{
   public  class Physician
    {
		public string physicianId { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string middleName { get; set; }
		public string suffix { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string zip { get; set; }
		public string hPhone { get; set; }
		public string fax { get; set; }
		public string ssn { get; set; }
		public string license { get; set; }
		public string deaNumber { get; set; }
		public string email { get; set; }
		public string npi { get; set; }
		public string wPhone { get; set; }
		public string mobile { get; set; }
		public string loginName { get; set; }
		public string speciality { get; set; }
		public string type { get; set; }
	}
}
