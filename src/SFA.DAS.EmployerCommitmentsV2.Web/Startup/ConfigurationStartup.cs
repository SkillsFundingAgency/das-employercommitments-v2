using Microsoft.AspNetCore.Hosting;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class ConfigurationStartup
    {
        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c
                .AddAzureTableStorage(
                    Configuration.ConfigurationKeys.Encoding,
                    Configuration.ConfigurationKeys.EmployerCommitmentsV2,
                    Commitments.Shared.Configuration.ConfigurationKeys.CommitmentsSharedConfiguration,
                    Configuration.ConfigurationKeys.EmployerUrlHelperConfiguration));
        }
    }
}