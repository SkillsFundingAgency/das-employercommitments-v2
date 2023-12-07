using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IAzureTableStorageConnectionAdapter, AzureTableStorageConnectionAdapter>();
        services.AddTransient<IEnvironmentService, EnvironmentService>();
        services.AddTransient<IAutoConfigurationService, TableStorageConfigurationService>();
        
        services.AddSingleton<ICurrentDateTime, CurrentDateTime>();
        services.AddSingleton<ICreateCsvService, CreateCsvService>();
        
        return services;
    }
}