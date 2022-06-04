using MedisoftFhirAgent.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Interfaces
{
    public interface ILoggerRepository
    {
        void Log(log _log);

    }
}
