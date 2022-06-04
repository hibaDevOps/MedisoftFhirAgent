using MedisoftFhirAgent.DatabaseContexts;
using MedisoftFhirAgent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Repositories
{
    class MergeRepository : IMergeRepository
    {
        private readonly AdvantageContext _context;

        public MergeRepository()
        {
            _context = new AdvantageContext();
        }
        public MergeRepository(AdvantageContext context)
        {
            _context = context;
        }
        public void MergeMedisoftPatient()
        {
           _context.mergeMedisoft();
        }
    }
}
