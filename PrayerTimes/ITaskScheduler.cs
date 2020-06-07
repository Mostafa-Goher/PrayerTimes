using System;

namespace PrayerTimes.BackgroundWorker
{
    public interface ITaskScheduler: IDisposable
    {
        void ScheduleTask(int hour, int min, Action task);
    }
}
