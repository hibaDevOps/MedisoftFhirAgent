using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Interfaces
{
    interface IScheduler
    {
        public void logSchedulerService(DateTime dt_start, DateTime dt_end);

    }
}
