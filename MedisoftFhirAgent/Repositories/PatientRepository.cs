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
                    inBound.Payload = inBound.Payload.Replace(System.Environment.NewLine, string.Empty);

                    Patient p = JsonSerializer.Deserialize<Patient>(inBound.Payload);
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
            List<Patient> newPatients = new List<Patient>();
            Debug.WriteLine(lisofPatients);
            if (lisofPatients != null)
            {
                foreach (var pat in lisofPatients)
                {
                    if (_context.findPatient(pat))
                    {
                        _context.updatePatient(pat);
                    }
                    else
                    {
                        newPatients.Add(pat);
                    }
                }
                if (newPatients.Count() > 0)
                {
                    _context.insertIntoPatients(newPatients);
                    _context.createInboundRecord(lisofPatients);
                }
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
        public bool logFailedRecords(string identifier, string type, string message)
        {
            return _context.logFailedRecords(identifier, type, message);
        }
        public bool migrationConfirmed(Patient obj)
        {
            Debug.WriteLine(obj.Identifier);
            if (_context.findMigratedPatient(obj))
            {
                if (_context.setMigrationConplete(obj))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<Patient> GetAllDeletedPatients()
        {
            return _context.DeletedPatients();
        }
    }
}
