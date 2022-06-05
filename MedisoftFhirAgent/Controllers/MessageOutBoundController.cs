using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using MedisoftFhirAgent.Interfaces;
using MedisoftFhirAgent.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Controllers
{
    public class MessageOutBoundController
    {
        static HttpClient client = new HttpClient();
        private MessageOutBoundRepository _ipr;
        private PatientController _pr;
        private LoggingController _lgc;
        public MessageOutBoundController()
        {
            _pr = new PatientController();
            _ipr = new MessageOutBoundRepository();
            _lgc = new LoggingController();
        }
        public MessageOutBoundController(MessageOutBoundRepository mRepo)
        {
            _ipr = mRepo;

        }
       public async Task<MessageQueueOutBound> sendPatientData()
        {
           return await _ipr.sendToIntegrationAsync(_pr.GetAllPatients());
        }
      
 

    }
}
