using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.DAL.Entities.MessagesQueue
{
    public class MessageQueueOutBound
    {
        public string Id { get; set; }
        public string CreatedDate { get; set; }
        public string ProcessingDate { get; set; }
        public string Error { get; set; }
        public string Payload { get; set; }
        public string ResourceType { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}
