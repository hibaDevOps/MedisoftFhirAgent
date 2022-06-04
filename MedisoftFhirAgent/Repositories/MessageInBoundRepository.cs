using MedisoftFhirAgent.Controllers;
using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using MedisoftFhirAgent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Repositories
{
    public class MessageInBoundRepository : IMessageInBoundRepository<MessageQueueInBound>
    {
        private LoggingController _ipr;
        private PatientController _ptr;

        public MessageInBoundRepository()
        {
            _ipr = new LoggingController();
            _ptr = new PatientController();
        }
        public async Task<List<MessageQueueInBound>> getMessageQueueInBound()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {


                    var url = "https://localhost:44393/MessageQueue/pull?sourceSystem=Medisoft";

                    var response = await client.GetAsync(url);

                    string result = response.Content.ReadAsStringAsync().Result;
                    if (result !="")
                    {
                       // List<MessageQueueInBound> allData = JsonSerializer.Deserialize<List<MessageQueueInBound>>(result);
                        _ipr.Log("Get_Medisoft_pull_API", result);
                        //  string jsonSample = "[{\"id\":\"\",\"createdDate\":\"2022-05-17\",\"type\":\"I\",\"processingDate\":\"\",\"payload\":\"[{\"identifier\":\"PAT0001\",\"prefix\":\"Mr\",\"firstName\":\"ABC\",\"lastName\":\"XYZ\",\"birthDate\":\"1991-02-15\",\"birthPlace\":\"NorthCarolina\",\"citizenshipCode\":\"US\",\"gender\":\"Male\",\"address\":{\"streetName\":\"string\",\"streetNo\":\"string\",\"appartmentNo\":\"string\",\"postalCode\":\"27703\",\"city\":\"NorthCarolina\",\"country\":\"US\",\"type\":\"string\"}}]\",\"error\":\"\",\"resourceType\":\"Patient\",\"source\":\"MediSoft\",\"status\":\"\"}]";
                        string jsonSample = "[{\"id\":\"\",\"createdDate\":\"2022-05-17\",\"type\":\"I\",\"processingDate\":\"\",\"payload\":{\"identifier\":\"HOLIIDAY890\",\"prefix\":\"Mr\",\"firstName\":\"ABC\",\"lastName\":\"XYZ\",\"birthDate\":\"1991-02-15\",\"birthPlace\":\"NorthCarolina\",\"citizenshipCode\":\"US\",\"gender\":\"Male\",\"address\":{\"streetName\":\"string\",\"streetNo\":\"string\",\"appartmentNo\":\"string\",\"postalCode\":\"27703\",\"city\":\"NorthCarolina\",\"country\":\"US\",\"type\":\"string\"}},\"error\":\"\",\"resourceType\":\"Patient\",\"source\":\"MediSoft\",\"status\":\"\"}]";
                        _ptr.savePatients(jsonSample);
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("Internal server Error");
                        return null;
                    }
                }
            }
        }

      
    }
}
