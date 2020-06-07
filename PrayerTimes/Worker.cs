using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrayerTimes.BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITaskScheduler taskScheduler;

        public Worker(ILogger<Worker> logger, ITaskScheduler taskScheduler)
        {
            _logger = logger;
            this.taskScheduler = taskScheduler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            var soon = DateTime.Now.AddMinutes(1);
            var soon2 = soon.AddMinutes(1);

            taskScheduler.ScheduleTask(soon.Hour, soon.Minute, 
                () =>
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                });

            taskScheduler.ScheduleTask(soon2.Hour, soon2.Minute,
                () =>
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                });
        }
    }
}
