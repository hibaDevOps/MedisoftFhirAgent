using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.DAL.Entities.Appointment
{
    public class Appointment
    {
        public int id { get; set; }

        public string name { get; set; }
        public string date { get; set; }
        public DateTime start_time { get; set; }
        public int length { get; set; } 
        public string provider { get; set; }
        public string chart_number { get;set; }
        public int case_number { get; set; }
        public string status { get; set; }
        public DateTime check_out_time { get; set; }
        public DateTime check_in_time { get; set; }

        public string note { get; set; }        
        public List<ServiceCategory> serviceCategory { get; set; }
         public List<AppointmentSpeciality> speciality { get; set; }
        public List<ServiceType> serviceType { get; set; }
        public AppointmentType appointmentType { get; set; }
        public List<Reason> reason { get; set; }
        public RequestedPeriod requestedPeriod { get; set; }
        public List<Participant> participant { get; set; }


    }
}
