using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrayerTimes.BackgroundWorker
{
    public class PrayerTimesHTTPClient: IPrayerTimesClient
    {
        private const string url = "http://api.aladhan.com/v1/timingsByCity?city=Tilburg&country=Netherlands&method=3";
        private readonly ILogger<PrayerTimesHTTPClient> logger;

        public PrayerTimesHTTPClient(ILogger<PrayerTimesHTTPClient> logger)
        {
            this.logger = logger;
        }

        public async Task<PrayerTimesTimings> GetTodayTimings()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync(new Uri(url));
                    var result = JsonConvert.DeserializeObject<PrayerTimesReponse>(response);
                    return result.Data.PrayerTimesTimings;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error when calling {url}", url);
                    return null;
                }
            }
        }
    }
}
