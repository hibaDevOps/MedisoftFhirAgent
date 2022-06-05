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

        public async Task<List<MessageQueueInBound>> verify()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {


                    var url = "https://localhost:44393/MessageQueue/Verify?sourceSystem=MediSoft";

                    var response = await client.GetAsync(url);

                    string result = response.Content.ReadAsStringAsync().Result;
                    if (result != "")
                    {
                        List<MessageQueueInBound> allData = JsonSerializer.Deserialize<List<MessageQueueInBound>>(result);
                        _ipr.Log("Verify_API", result);
                        return allData;
                    }
                    else
                    {
                        Console.WriteLine("Internal server Error");
                        return null;
                    }
                }
            }

        }

        public async Task<List<MessageQueueInBound>> verifyIntegration()
        {
            List<MessageQueueInBound> _msgQueue = new List<MessageQueueInBound>();
            _msgQueue = await verify();
            if (_msgQueue != null)
            {
                foreach (var queue in _msgQueue)
                {
                    if (queue.Fhirid != "")
                    {
                        if (_ptr.migrationConfirmed(queue.Payload))
                        {
                            queue.Status = "C";
                        }
                    }
                    else
                    {
                        queue.Status = "F";
                    }
                }
            }
            return _msgQueue;
        }

        public async Task<bool> updateInbounddStatus()
        {
            List<MessageQueueInBound> _inList = new List<MessageQueueInBound>();
            List<MessageQueueOutBound> _outList = new List<MessageQueueOutBound>();
            _inList = await verifyIntegration();
            foreach(var lst in _inList) {
                MessageQueueOutBound ms = new MessageQueueOutBound();
                ms.Id = lst.Id;
                ms.MessageQueueId = lst.MessageQueueId;
                ms.ProcessingDate = lst.ProcessingDate;
                ms.CreatedDate = lst.CreatedDate;
                ms.Status = lst.Status;
                ms.Type = lst.Type;
                ms.Source = lst.Source;
                ms.ResourceType = lst.ResourceType;
                ms.Payload = JsonSerializer.Serialize(lst.Payload);
                _outList.Add(ms);
            }

            if(_outList.Count() > 0)
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {

                        var json = JsonSerializer.Serialize(_outList);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        var url = "https://localhost:44393/MessageQueue/inbound/update";

                        var response = await client.PostAsync(url, data);

                        string result = response.Content.ReadAsStringAsync().Result;
                        if (result == "true")
                        {
                            return true;
                        }
                        else
                        {
                            _ipr.Log("Update_MessageQueue_Push_API_Issues", result.ToString());
                            return false;
                        }
                    }
                }

            }
            return false;
        }
        

    }
}
