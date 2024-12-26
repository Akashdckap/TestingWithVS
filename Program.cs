using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System;
using DotNetEnv;

namespace P21_latest_template
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()//Logs all messages with level Debug or higher it means multiple level options we have like information,warning,error etc if we give Debug it will log all the messages
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)//Ensures that log from Microsoft internal components are also at least debug level. For Microsoft components, I want the logging level to be at least Debug."
                .Enrich.FromLogContext()//Add extra information for contextual
                .WriteTo.File("logs\\logs.txt",
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),//Logs are written to disk every second
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)//it's sets the level to start
                .CreateLogger();

            try
            {
                BuildWebHost(args).Build().Run();
            }
            catch (Exception)
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder BuildWebHost(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
