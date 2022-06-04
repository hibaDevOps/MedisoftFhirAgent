using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.Interfaces;
using MedisoftFhirAgent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Controllers
{
    class LoggingController
    {
        private ILoggerRepository _ipr;
        public LoggingController()
        {
            _ipr = new LoggingRepository();
        }
        public LoggingController(ILoggerRepository iLog)
        {
            _ipr = iLog;
        }
        public void Log(string source, string message)
        {
            log _log = new log();
            _log.LogTime = DateTime.Now;
            _log.message = message;
            _log.source = source;
            _ipr.Log(_log);
        }
    }
}
