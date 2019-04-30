using Microsoft.AspNetCore.Hosting;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerCommitmentsV2.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class ConfigurationStartup
    {
        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c
                .AddAzureTableStorage(ConfigurationKeys.EmployerCommitmentsV2));
        }
    }
}