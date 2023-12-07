using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;

namespace SFA.DAS.EmployerCommitmentsV2.Web;

public static class Program
{
    public static void Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var logger = LogManager.Setup().LoadConfigurationFromXml(environment == "Development" ? "nlog.Development.config" : "nlog.config").GetCurrentClassLogger();
        logger.Info("Starting up host");

        CreateWebHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseNLog();
            });
}