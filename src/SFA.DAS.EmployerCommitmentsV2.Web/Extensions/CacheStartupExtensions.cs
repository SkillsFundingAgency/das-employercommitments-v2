using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerCommitmentsV2.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class CacheStartupExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {
            var redisConfiguration = configuration.GetSection(ConfigurationKeys.ConnectionStrings)
           .Get<EmployerCommitmentsV2Settings>();

            if (redisConfiguration == null)
            {
                return services;
            }

            if (environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(
                    options => { options.Configuration = redisConfiguration.RedisConnectionString; });
            }

            return services;
        }
    }
}
