using MedisoftFhirAgent.Controllers;
using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using MedisoftFhirAgent.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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


                    var url = "https://yeatspatientportal-integration-staging.azurewebsites.net/MessageQueue/pull?sourceSystem=Medisoft";

                    var response = await client.GetAsync(url);

                    string result = response.Content.ReadAsStringAsync().Result;
                   
                   // result = "[{\"id\":\"11\",\"messsgeQueueId\":\"15\",\"createdDate\":\"5/17/202212:00:00AM\",\"type\":\"I\",\"processingDate\":\"5/28/20228:32:40PM\",\"payload\":\"{\\r\\n\\\"identifier\\\":\\\"TEST20\\\",\\r\\n\\\"prefix\\\":\\\"Ms\\\",\\r\\n\\\"firstName\\\":\\\"hiba Sheikh\\\",\\r\\n\\\"lastName\\\":\\\"Sheikh\\\",\\r\\n\\\"birthDate\\\":\\\"1991-02-15\\\",\\r\\n\\\"birthPlace\\\":\\\"NorthCarolina\\\",\\r\\n\\\"citizenshipCode\\\":\\\"US\\\",\\r\\n\\\"gender\\\":\\\"Male\\\",\\r\\n\\\"address\\\":{\\r\\n\\\"streetName\\\":\\\"St1\\\",\\r\\n\\\"streetNo\\\":\\\"18\\\",\\r\\n\\\"appartmentNo\\\":\\\"1008\\\",\\r\\n\\\"postalCode\\\":\\\"27703\\\",\\r\\n\\\"city\\\":\\\"NorthCarolina\\\",\\r\\n\\\"country\\\":\\\"US\\\",\\r\\n\\\"type\\\":\\\"string\\\"\\r\\n}\\r\\n}\",\"error\":\"\",\"resourceType\":\"Patient\",\"source\":\"MediSoft\",\"fhirid\":\"MEDINT0006\",\"status\":\"VC\"}]"; 
                    if (result !="")
                    {

                        _ipr.Log("Get_Medisoft_pull_API", result);
                         _ptr.processPatientRecords(result);
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


                    var url = "https://yeatspatientportal-integration-staging.azurewebsites.net/MessageQueue/Verify?sourceSystem=MediSoft";

                    var response = await client.GetAsync(url);

                    string result = response.Content.ReadAsStringAsync().Result;
                    _ipr.Log("Verify_API_result", result);


                    if (result != "")
                    {
                        result = result.Replace(System.Environment.NewLine, string.Empty);
                        List<MessageQueueInBound> allData = JsonConvert.DeserializeObject<List<MessageQueueInBound>>(result);
                        _ipr.Log("Verify_API", JsonConvert.SerializeObject(allData));
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
                Debug.WriteLine(_msgQueue);

                foreach (var queue in _msgQueue)
                {
                    if (queue.Status == "VC")
                    {
                        Debug.WriteLine(queue.Fhirid);
                        if (_ptr.migrationConfirmed(JsonConvert.DeserializeObject<Patient>(queue.Payload)))
                        {
                            Debug.WriteLine("Fhir found but record found in migrated records");
                            Debug.WriteLine(queue.ResourceType);
                            queue.Status = "C";
                        }
                     
                    }
                    else
                    {
                         
                        if (_ptr.migrationFailed(JsonConvert.DeserializeObject<Patient>(queue.Payload))){
                            queue.Status = "F";
                            _ptr.logFailedRecords(queue.Id, "Patient", "Patient records not Verified");
                        }

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
            _ipr.Log("veriyIntegrationUpdate", JsonConvert.SerializeObject(_inList));
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
                 ms.Payload = JsonConvert.SerializeObject(lst.Payload);
                 _outList.Add(ms);
             }

             if(_outList.Count() > 0)
             {
                 using (var httpClientHandler = new HttpClientHandler())
                 {
                     httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                     using (var client = new HttpClient(httpClientHandler))
                     {

                         var json = JsonConvert.SerializeObject(_outList);
                         var data = new StringContent(json, Encoding.UTF8, "application/json");

                         var url = "https://yeatspatientportal-integration-staging.azurewebsites.net/MessageQueue/inbound/update";

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
