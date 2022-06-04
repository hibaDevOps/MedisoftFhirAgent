using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DAL.Entities.MessagesQueue;
using MedisoftFhirAgent.DatabaseContexts;
using MedisoftFhirAgent.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Repositories
{
    class PatientRepository : IPatientRepository, ICommonRepository<Patient>
    {
        private readonly AdvantageContext _context;
        private MessageInBoundRepository _msgRepo;
        public PatientRepository()
        {
            _context = new AdvantageContext();
            _msgRepo = new MessageInBoundRepository();
        }
        public PatientRepository(AdvantageContext context)
        {
            _context = context;
        }
        public List<Patient> GetAll()
        {
            //return _context.Patients();
            return _context.Patients();
        }
        public List<Patient> GetAllUpdated()
        {
            return _context.UpdatedPatients();
        }
        public string convertListToPayload(List<Patient> listOfPatient)
        {
            var json = JsonSerializer.Serialize(listOfPatient);
            return json;

        }
        public List<Patient> getPatientsDataFromJson(List<MessageQueueInBound> msgB)
        {
            List<Patient> _pr = new List<Patient>();

            foreach (var inBound in msgB)
            {
                if (inBound.Payload != null)
                {
                    Patient p = inBound.Payload;
                    _pr.Add(p);
                  
                }
            }

            return _pr;
        }
        public string LogPatients()
        {
            return convertListToPayload(GetAll());
;        }
        public bool savePatients(List<Patient> lisofPatients)
        {
            Debug.WriteLine(lisofPatients);
            if (lisofPatients != null)
            {
                _context.insertIntoPatients(lisofPatients);
                return true;
            }
            return false;
        }
        public bool setDataMigrationStatus(List<Patient> lisOfPayload)
        {
            return _context.setPatientDataMigrationStatus(lisOfPayload);
        }
        public bool setUpdatedDataMigrationStatus(List<Patient> lisOfPayload)
        {
            return _context.setPatientUpdatedDataMigrationStatus(lisOfPayload);
        }
    }
}
