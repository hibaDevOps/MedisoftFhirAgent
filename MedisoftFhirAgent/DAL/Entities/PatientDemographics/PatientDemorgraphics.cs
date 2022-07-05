using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.DAL.Entities
{
    public class PatientDemorgraphics
    {
		public Patient patient {get; set;}
		public Patient guarantor {get; set;}
		public string guarantorRelationShip {get; set;}
		public Facility defaultFacility {get; set;}
		public List<Physician> physicians {get; set;}
		public Physician physician {get; set;}
		public List<Insurance> insuranceList {get; set;}

	
	}
}
