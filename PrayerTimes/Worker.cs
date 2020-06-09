using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreAudio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrayerTimes.BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITaskScheduler taskScheduler;
        private readonly IPrayerTimesClient prayerTimesClient;
        Player player;
        public Worker(ILogger<Worker> logger, ITaskScheduler taskScheduler, IPrayerTimesClient prayerTimesClient)
        {
            _logger = logger;
            this.taskScheduler = taskScheduler;
            this.prayerTimesClient = prayerTimesClient;
            player = new Player();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            InitForToday();
        }
        private void InitForToday()
        {
            _logger.LogInformation("Worker for today running at: {time}", DateTimeOffset.Now);
            var timings = prayerTimesClient.GetTodayTimings().Result;

            SchedulePrayer(timings.Fajr, nameof(timings.Fajr));
            SchedulePrayer(timings.Dhuhr, nameof(timings.Dhuhr));
            SchedulePrayer(timings.Asr, nameof(timings.Asr));
            SchedulePrayer(timings.Maghrib, nameof(timings.Maghrib));
            SchedulePrayer(timings.Isha, nameof(timings.Isha));

            var tomorrowsRun = TimeSpan.FromDays(1); // run tomorrow

            taskScheduler.ScheduleTask(tomorrowsRun.Hours, tomorrowsRun.Minutes,
                () =>
                {
                    InitForToday();
                });
        }

        private void SchedulePrayer(TimeSpan time, string prayerName)
        {
            DateTime now = DateTime.Now;

            DateTime prayertime = new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, 0, 0);
            if (prayertime <= now)
                return;

            taskScheduler.ScheduleTask(time.Hours, time.Minutes,
               () =>
               {
                   _logger.LogInformation("Worker for {prayer} running at: {time}", prayerName, DateTimeOffset.Now);
                   player.Play(@"http://praytimes.org/audio/adhan/Sunni/Abdul-Basit.mp3").RunSynchronously();
               });
        }
    }
}
