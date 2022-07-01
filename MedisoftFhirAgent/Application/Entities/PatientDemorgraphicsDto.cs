using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Application.Entities
{
    public class PatientDemorgraphicsDto
    {
		public PatientDto patient {get; set;}
		public PatientDto guarantor {get; set;}
		public string guarantorRelationShip {get; set;}
		public FacilityDto defaultFacility {get; set;}
		public List<PhysicianDto> physicians {get; set;}
		public PhysicianDto physician {get; set;}
		public List<InsuranceDto> insuranceList {get; set;}

	
	}
}
