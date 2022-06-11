using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using MedisoftFhirAgent.Interfaces;
using MedisoftFhirAgent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Controllers
{
    public class MessageInBoundController
    {
        private MessageOutBoundRepository _ipr;
        private PatientController _pr;
        private MessageInBoundRepository _iBound;
        public MessageInBoundController()
        {
            _pr = new PatientController();
            _ipr = new MessageOutBoundRepository();
            _iBound = new MessageInBoundRepository();
        }
        public MessageInBoundController(MessageOutBoundRepository mRepo)
        {
            _ipr = mRepo;

        }
        public async Task<List<MessageQueueInBound>> savePatientToMedisoft()
        {
            string obj = "{\"id\":\"\",\"createdDate\":\"2022-05-17\",\"type\":\"I\",\"processingDate\":\"\",\"payload\":\"[{}]\",\"error\":\"\",\"resourceType\":\"Patient\",\"source\":\"MediSoft\",\"status\":\"\"}";

            return await _iBound.getMessageQueueInBound();
        }
        public Task<MessageQueueOutBound> sendPatientData()
        {
            return _ipr.sendToIntegrationAsync(_pr.GetAllPatients());
        }

        public Task<bool> updateInboundStatus()
        {
            return _iBound.updateInbounddStatus();
        }
    }
}
