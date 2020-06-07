using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PrayerTimes.BackgroundWorker
{
    public class PrayerTimesReponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("timings")]
        public PrayerTimesTimings PrayerTimesTimings { get; set; }
    }

    public class PrayerTimesTimings
    {
        public TimeSpan Fajr { get; set; }
        public TimeSpan Dhuhr { get; set; }
        public TimeSpan Asr { get; set; }
        public TimeSpan Maghrib { get; set; }
        public TimeSpan Isha { get; set; }
    }
}
