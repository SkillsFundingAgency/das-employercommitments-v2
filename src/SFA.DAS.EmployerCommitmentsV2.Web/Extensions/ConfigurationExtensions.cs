using System.IO;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerCommitmentsV2.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class ConfigurationExtensions
{
    public static IConfiguration BuildDasConfiguration(this IConfiguration configuration)
    {
        var config = new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .SetBasePath(Directory.GetCurrentDirectory());

#if DEBUG
        if (!configuration.IsDev())
        {
            config.AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true);
        }
#endif

        config.AddEnvironmentVariables();
        
        config.AddAzureTableStorage(options =>
            {
                options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                options.EnvironmentName = configuration["EnvironmentName"];
                options.PreFixConfigurationKeys = true;
                options.ConfigurationKeysRawJsonResult = new[] { ConfigurationKeys.Encoding };
            }
        );

        return config.Build();
    }
    
    public static bool UseGovUkSignIn(this IConfiguration configuration)
    {
        return configuration[$"{ConfigurationKeys.EmployerCommitmentsV2}:UseGovSignIn"] != null &&
               configuration[$"{ConfigurationKeys.EmployerCommitmentsV2}:UseGovSignIn"]
                   .Equals("true", StringComparison.CurrentCultureIgnoreCase);
    }
    
    public static bool IsDev(this IConfiguration configuration) => configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
    
    public static bool UseLocalRegistry(this IConfiguration configuration)
    {
        var isLocalRegistry = configuration.GetValue<bool?>("UseLocalDevRegistry");

        return isLocalRegistry != null && isLocalRegistry.Value;
    }
}