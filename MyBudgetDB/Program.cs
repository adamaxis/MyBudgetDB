using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MyBudgetDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            BuildWebHost(args).Run();
            Log.CloseAndFlush();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .ConfigureAppConfiguration(config =>
                    config.AddJsonFile("appsettings.json"))
                .ConfigureLogging((ctx, builder) =>
                {
                    builder.AddConfiguration(
                        ctx.Configuration.GetSection("Logging"));
                })
                .Build();
    }
}
