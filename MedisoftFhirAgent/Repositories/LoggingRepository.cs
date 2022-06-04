using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.Repositories
{
    public class LoggingRepository : ILoggerRepository
    {
        public LoggingRepository()
        {

        }
        public void WriteToFile(log _log)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + _log.source + "Log_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(_log.LogTime + " : " + _log.source + " - " + _log.message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(_log.LogTime + " : " + _log.source + " - " + _log.message);
                }
            }
        }

        public void Log(log _log)
        {
            WriteToFile(_log);
        }
     
    }
}
