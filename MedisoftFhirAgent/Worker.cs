using MedisoftFhirAgent.Controllers;
using MedisoftFhirAgent.DatabaseContexts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MedisoftFhirAgent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private LoggingController _log;
        private MessageOutBoundController _pat;
        private MessageInBoundController _InData;
        private SchedulerController _sch;
        private DateTime dt_start;
        private MergeController _mg;


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {

            _log = new LoggingController();
            _pat = new MessageOutBoundController();
            _InData = new MessageInBoundController();
            _sch = new SchedulerController();
             dt_start = DateTime.Now;
            _mg = new MergeController();
            _log.Log("Medisoft Database", "Service is sarted at: " +dt_start);


            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
             //   dt_end = DateTime.Now;
                _log.Log("Medisoft Database", "Service is recalled at: " + DateTime.Now);
                // _sch.logScheduler(dt_start, dt_end);
              // var lst = _InData.savePatientToMedisoft();
              //  string patLog = JsonSerializer.Serialize(_pat.sendPatientData());
               //_log.Log("Medisoft Database", patLog + DateTime.Now);
              //    string patUpdLog = JsonSerializer.Serialize(_pat.sendUpdatedPatientData());
              //   _log.Log("Medisoft Updated Patients", patUpdLog + DateTime.Now);

               // //  _medisoft.getAllPatients();
                _mg.mergeMedisoftPatients();
               // await _InData.updateInboundStatus();

                //dt_start = dt_end;
                await Task.Delay(60000, stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _log.Log("Medisoft Database", "Service is stopped at: " + DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}
