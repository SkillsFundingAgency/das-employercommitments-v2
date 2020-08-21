﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Configurations;
using StackExchange.Redis;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class DataProtectionStartupExtensions
    {
        public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                var redisConnectionString = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
                    .Get<EmployerCommitmentsV2Settings>().RedisConnectionString;

                var defaultDatabase = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
                    .Get<EmployerCommitmentsV2Settings>().DefaultDatabase;

                var redis = ConnectionMultiplexer
                    .Connect($"{redisConnectionString}, {defaultDatabase}");

                services.AddDataProtection()
                    .SetApplicationName("das-employercommitments-v2")
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
            }
            return services;
        }
    }
}