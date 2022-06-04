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
        public List<Patient> GetAllUpdatedPatients()
        {
            return _ipr.GetAllUpdated();
        }
        public List<MessageQueueInBound> MapDataToSource(string dataJson)
        {

            List<MessageQueueInBound> msgObj = JsonConvert.DeserializeObject<List<MessageQueueInBound>>(dataJson);
            return msgObj;

        }
        public bool savePatients(string obj)
        {
            Debug.WriteLine(this.MapDataToSource(obj));
            _ipr.savePatients(_ipr.getPatientsDataFromJson(this.MapDataToSource(obj)));
            return true;
        }
        public string LogPatients()
        {
            return _ipr.LogPatients();
        }
    }
}
