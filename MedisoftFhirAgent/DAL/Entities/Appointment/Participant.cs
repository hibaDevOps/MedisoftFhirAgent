using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.DAL.Entities.Appointment
{
    public class Participant
    {
        public List<ParticipantType> type { get; set; }
        public ParticipantActors actor { get; set; }
        public string required { get; set; }
        public string participantStatus { get; set; }
     }
}
