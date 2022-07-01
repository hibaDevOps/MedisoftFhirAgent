using MedisoftFhirAgent.Interfaces;
using System;
using MedisoftFhirAgent.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DatabaseContexts;
using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using System.Text.Json;
using System.Diagnostics;
using Newtonsoft.Json;
using MedisoftFhirAgent.Application.Constants;

namespace MedisoftFhirAgent.Controllers
{
    class PatientController
    {
        private IPatientRepository _ipr;
        public PatientController()
        {
            _ipr = new PatientRepository(new AdvantageContext());
        }
        public PatientController(IPatientRepository patientRepository)
        {
            _ipr = patientRepository;
        }
        public List<Patient> GetAllPatients()
        {
            return _ipr.GetAll();
        }

        public List<Patient> GetAllDeletedPatients()
        {
            return _ipr.GetAllDeletedPatients();
        }
        public List<Patient> GetAllUpdatedPatients()
        {
            return _ipr.GetAllUpdated();
        }
        public List<MessageQueueInBound> MapDataToSource(string dataJson)
        {

            List<MessageQueueInBound> msgObj = JsonConvert.DeserializeObject<List<MessageQueueInBound>>(dataJson);
            Debug.WriteLine("test exception");
            return msgObj;

        }
        public bool logFailedRecords(string identifier, string type, string message)
        {
            return _ipr.logFailedRecords(identifier, type, message);
        }
        public bool processPatientRecords(string obj)
        {
          

            List<MessageQueueInBound> saveRecords = new List<MessageQueueInBound>();
            List<MessageQueueInBound> deletedRecords = new List<MessageQueueInBound>();
            foreach(var inBound in MapDataToSource(obj))
            {
                if(inBound.ResourceType == Constants.DeletedPatientsResourceType) {
                    deletedRecords.Add(inBound);
                }
                else
                {
                    saveRecords.Add(inBound);
                }
            }
            _ipr.savePatients(_ipr.getPatientsDataFromJson(saveRecords));
            _ipr.deletePatients(_ipr.getPatientsDataFromJson(deletedRecords));
            return true;
        }
        public string LogPatients()
        {
            return _ipr.LogPatients();
        }
        public bool migrationConfirmed(Patient obj)
        {
            return _ipr.migrationConfirmed(obj);
        }
        public bool migrationFailed(Patient obj)
        {
            return _ipr.migrationFailed(obj);
        }
    }
}
