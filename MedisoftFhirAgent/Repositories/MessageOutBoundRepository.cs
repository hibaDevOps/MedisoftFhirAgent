using MedisoftFhirAgent.Controllers;
using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using MedisoftFhirAgent.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Repositories
{
    public class MessageOutBoundRepository : IMessageOutBoundRepository<Patient> 
    {
        private PatientRepository _ipr;
        private LoggingController _lgc;
        public MessageOutBoundRepository()
        {
            _ipr = new PatientRepository();
            _lgc = new LoggingController();
        }
      
        public async Task<MessageQueueOutBound> sendToIntegrationAsync(List<Patient> listOfObject)
        {


            if (listOfObject != null)
            {
                DateTime dt = DateTime.Now;
                MessageQueueOutBound msgQueue = new MessageQueueOutBound();
                msgQueue.CreatedDate = String.Format("{0:yyyy-MM-dd}", dt);  
                msgQueue.Error = "";
                msgQueue.Id = "";
                msgQueue.Payload = convertListToPayload(listOfObject);
                msgQueue.ProcessingDate = "";
                msgQueue.Source = "MediSoft";
                msgQueue.Status = "";
                msgQueue.ResourceType = "Patient";
                msgQueue.Type = "I";



                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {

                        var json = JsonSerializer.Serialize(msgQueue);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        var url = "https://localhost:44393/MessageQueue/push";

                        Debug.WriteLine(data.ReadAsStringAsync().Result);
                        var response = await client.PostAsync(url, data);

                        string result = response.Content.ReadAsStringAsync().Result;
                        if (result == "true")
                        {
                            this.sendPatientDataMigration(listOfObject);  //changes the status of the migrated data
                        }
                        else
                        {
                            _lgc.Log("Patient_MessageQueue_Push_API_Issues", result.ToString());
                        }
                    }
                }




                return msgQueue;

            }
            return null;

        }

        public string convertListToPayload(List<Patient> listToPayload)
        {
            var json = JsonSerializer.Serialize(listToPayload);
            return json;
        }

        public bool sendPatientDataMigration(List<Patient> pr)
        {
            return _ipr.setDataMigrationStatus(pr);
        }
        public bool sendUpdatedDataMigration(List<Patient> pr)
        {
            return _ipr.setUpdatedDataMigrationStatus(pr);
        }

     
    }
}
