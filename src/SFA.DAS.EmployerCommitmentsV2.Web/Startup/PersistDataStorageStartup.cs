using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Configurations;
using StackExchange.Redis;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class PersistDataStorageStartup
    {
        public static IServiceCollection SaveKeysToStackExchangeRedis(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            if (!isDevelopment)
            {
                var redisConnectionString = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
                    .Get<EmployerCommitmentsV2Settings>().RedisConnectionString;

                var redis = ConnectionMultiplexer
                    .Connect($"{redisConnectionString}, DefaultDatabase=3");

                services.AddDataProtection()
                    .SetApplicationName("das-employercommitments-v2")
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
            }
            return services;
        }
    }
}