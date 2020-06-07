using System.Threading.Tasks;

namespace PrayerTimes.BackgroundWorker
{
    public interface IPrayerTimesClient
    {
        Task<PrayerTimesTimings> GetTodayTimings();
    }
}
