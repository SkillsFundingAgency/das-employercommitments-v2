using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.EmployerCommitmentsV2.Web.Startup;

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
                .UseKestrel(o => o.AddServerHeader = false)
                .UseStartup<AspNetStartup>();
    }
}
