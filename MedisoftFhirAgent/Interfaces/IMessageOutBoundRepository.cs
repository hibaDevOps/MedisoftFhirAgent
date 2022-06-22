using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Interfaces
{
    public interface IMessageOutBoundRepository<T> where T:class
    {
        public  Task<MessageQueueOutBound>  sendToIntegrationAsync(List<T> listOfObject);

        public Task<MessageQueueOutBound> sendDeletedToIntegrationAsync(List<T> listOfObjects);
 }
}
