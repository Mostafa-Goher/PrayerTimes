using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreAudio;
using NetCoreAudio.Interfaces;

namespace PrayerTimes.BackgroundWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ITaskScheduler, TaskScheduler>();
                    services.AddSingleton<IPrayerTimesClient, PrayerTimesHTTPClient>();
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IPlayer, Player>();
                });
    }
}
