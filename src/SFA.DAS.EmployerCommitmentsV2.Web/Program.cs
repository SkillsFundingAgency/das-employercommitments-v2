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
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureDasAppConfiguration()
                .UseApplicationInsights()
                .UseNLog()
                .UseKestrel(o => o.AddServerHeader = false)
                .UseStructureMap()
                .UseStartup<AspNetStartup>();
    }
}
