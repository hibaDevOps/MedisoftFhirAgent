using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Interfaces
{
    interface IPatientRepository
    {
        public List<Patient> GetAll();
        public List<Patient> GetAllUpdated();
        public string LogPatients();
        public bool savePatients(List<Patient> lisofPatients);
        public List<Patient> getPatientsDataFromJson(List<MessageQueueInBound> msgB);

        public bool migrationConfirmed(Patient obj);
        public bool migrationFailed(Patient obj);

        public bool logFailedRecords(string identifier, string type, string message);

        public List<Patient> GetAllDeletedPatients();
    }
}
