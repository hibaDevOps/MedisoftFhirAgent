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
    class MergeController
    {
        private IMergeRepository _ipr;
        public MergeController()
        {
            _ipr = new MergeRepository(new AdvantageContext());
        }
        public MergeController(IMergeRepository mRepo)
        {
            _ipr = mRepo;
        }
        public void mergeMedisoftPatients()
        {
            _ipr.MergeMedisoftPatient();
        }
    }
}
