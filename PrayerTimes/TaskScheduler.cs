﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PrayerTimes.BackgroundWorker
{
    public class TaskScheduler : ITaskScheduler
    {
        private Dictionary<Guid, Timer> timers = new Dictionary<Guid, Timer>();
        private bool disposedValue;
        private readonly ILogger<TaskScheduler> logger;

        public TaskScheduler(ILogger<TaskScheduler> logger)
        {
            this.logger = logger;
        }

        public void ScheduleTask(int hour, int min, Action task)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);

            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            TimeSpan waitTime = firstRun - now;

            if (waitTime <= TimeSpan.Zero)
            {
                waitTime = TimeSpan.Zero;
            }

            var key = Guid.NewGuid();
            logger.LogInformation("Scheduling a task with key {key} that will run at: {time}", key, firstRun);
            logger.LogInformation("Scheduling a task that will run after: {time}", waitTime);

            var timer = new Timer(x =>
            {
                task.Invoke();
                FinalizeTimer(key);

            }, null, waitTime, TimeSpan.FromMilliseconds(-1));

            timers.Add(key,timer);
        }

        private void FinalizeTimer(Guid key)
        {
            timers[key].Dispose();
            timers.Remove(key);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var timer in timers)
                    {
                        timer.Value.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
