using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using SFA.DAS.EmployerCommitmentsV2.Web.Startup;
using StructureMap.AspNetCore;

namespace SFA.DAS.EmployerCommitmentsV2.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logger = NLogBuilder.ConfigureNLog(environment == "Development" ? "nlog.Development.config" : "nlog.config").GetCurrentClassLogger();
            logger.Info("Starting up host");

            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureDasAppConfiguration()
                .UseNLog()
                .UseStructureMap()
                .UseStartup<AspNetStartup>();
    }
}
