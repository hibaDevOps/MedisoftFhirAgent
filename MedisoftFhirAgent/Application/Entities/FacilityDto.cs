using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Application.Entities
{
    public class FacilityDto
    {
		public string facilityId { get; set; }
		public string name  { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string zipCode { get; set; }
		public string altPhone { get; set; }
		public string phone
        {
            get; set;

		 }
		public string fax
		{
			get; set;
		}
		public string email
        {
            get; set;
        }
		public string mobile
        {
			get;set;
        }
	}
}
