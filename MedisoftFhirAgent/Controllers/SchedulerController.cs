using MedisoftFhirAgent.DatabaseContexts;
using MedisoftFhirAgent.Interfaces;
using MedisoftFhirAgent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Controllers
{
    class SchedulerController
    {
        private IScheduler _ipr;
        public SchedulerController()
        {
            _ipr = new SchedulerRepository(new AdvantageContext());
        }
        public SchedulerController(IScheduler schedulerRepository)
        {
            _ipr = schedulerRepository;
        }
        public void logScheduler(DateTime dt_start, DateTime dt_end)
        {
            _ipr.logSchedulerService(dt_start,dt_end);
        }
    }
}
