using MedisoftFhirAgent.DatabaseContexts;
using MedisoftFhirAgent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Repositories
{
     class SchedulerRepository : IScheduler
    {
        private readonly AdvantageContext _adv;

        public SchedulerRepository()
        {
            _adv = new AdvantageContext();
        }

        public SchedulerRepository(AdvantageContext adv)
        {
            _adv = adv;
        }
        public void logSchedulerService(DateTime dt_start, DateTime dt_end)
        {
            _adv.logSchedulerService(dt_start, dt_end);
        }
    }
}
