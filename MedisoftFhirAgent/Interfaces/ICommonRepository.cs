using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Interfaces
{
    public interface ICommonRepository<T> where T: class
    {
        public string convertListToPayload(List<T> listToPayload);
        public bool setDataMigrationStatus(List<T> lisOfPayload);
        public bool setUpdatedDataMigrationStatus(List<T> listOfPayload);
    }
}
