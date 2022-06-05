using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Interfaces
{
    public interface IMessageInBoundRepository<T> where T: class
    {
        //public List<T> MapDataToSource(string dataJson);
        public Task<List<MessageQueueInBound>> getMessageQueueInBound();
        public Task<List<MessageQueueInBound>> verifyIntegration();

        public Task<bool> updateInbounddStatus();


    }
}
