using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Application.Entities
{
    public class InsuranceDto
    {
		public string providerCode {get; set;}
		public string providerName {get; set;}
		public string providerAddress1 {get; set;}
		public string providerAddress2 {get; set;}
		public string providerCity {get; set;}
		public string providerState {get; set;}
		public string providerZip {get; set;}
		public string providerPhone {get; set;}
		public string insuredPerson {get; set;}
		public string policyNumber {get; set;}
		public string groupNumber {get; set;}
		public string startDate {get; set;}
		public string endDate {get; set;}
		public string relationShip {get; set;}
		public PatientDto insuredPersonInfo {get; set;}
		public string order {get; set;}

	}
}
