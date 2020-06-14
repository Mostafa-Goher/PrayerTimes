using System;
using System.Threading.Tasks;

namespace PrayerTimes.BackgroundWorker
{
    public interface ITaskScheduler: IDisposable
    {
        void ScheduleTask(int hour, int min, Func<Task> task, string name = "worker");
    }
}
