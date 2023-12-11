﻿using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.CommitmentsV2.Services.Shared;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Services;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Services.Stubs;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        EmployerCommitmentsV2Configuration configuration)
    {
        services.AddTransient<IAzureTableStorageConnectionAdapter, AzureTableStorageConnectionAdapter>();
        services.AddTransient<IEnvironmentService, EnvironmentService>();
        services.AddTransient<IAutoConfigurationService, TableStorageConfigurationService>();

        services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
        services.AddSingleton<StubEmployerAccountsApiClient, StubEmployerAccountsApiClient>();

        if (configuration.UseStubEmployerAccountsApiClient)
        {
            services.AddTransient<IEmployerAccountsApiClient, StubEmployerAccountsApiClient>();
        }
        else
        {
            services.AddTransient<IEmployerAccountsApiClient, EmployerAccountsApiClient>();
        }
        
        services.AddSingleton(typeof(ICookieStorageService<>), typeof(CookieStorageService<>));
        services.AddSingleton<ICurrentDateTime, CurrentDateTime>();
        services.AddSingleton<ICreateCsvService, CreateCsvService>();
        services.AddSingleton<IAcademicYearDateProvider, AcademicYearDateProvider>();
        
        return services;
    }
}