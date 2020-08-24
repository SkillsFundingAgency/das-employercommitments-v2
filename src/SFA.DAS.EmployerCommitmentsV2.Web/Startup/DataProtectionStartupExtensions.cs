using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class DataProtectionStartupExtensions
    {
        public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                var redisConfiguration = configuration.GetSection(ConfigurationKeys.ConnectionStrings)
                    .Get<EmployerCommitmentsV2Settings>();

                if (redisConfiguration != null)
                {
                    var redisConnectionString = redisConfiguration.RedisConnectionString;
                    var dataProtectionKeysDatabase = redisConfiguration.DataProtectionKeysDatabase;

                    var redis = ConnectionMultiplexer
                        .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

                    services.AddDataProtection()
                        .SetApplicationName("das-employercommitments-v2")
                        .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
                }
            }
            return services;
        }
    }
}