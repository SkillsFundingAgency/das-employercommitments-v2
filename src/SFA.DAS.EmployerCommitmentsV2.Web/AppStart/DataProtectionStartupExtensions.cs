using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.EmployerCommitmentsV2.Web.AppStart;

public static class DataProtectionStartupExtensions
{
    public static IServiceCollection AddDasDataProtection(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return services;
        }
        
        var redisConfiguration = configuration.GetSection(ConfigurationKeys.ConnectionStrings)
            .Get<EmployerCommitmentsV2Settings>();

        if (redisConfiguration == null)
        {
            return services;
        }
        
        var redisConnectionString = redisConfiguration.RedisConnectionString;
        var dataProtectionKeysDatabase = redisConfiguration.DataProtectionKeysDatabase;

        var redis = ConnectionMultiplexer
            .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

        services.AddDataProtection()
            .SetApplicationName("das-employer")
            .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        
        return services;
    }
}