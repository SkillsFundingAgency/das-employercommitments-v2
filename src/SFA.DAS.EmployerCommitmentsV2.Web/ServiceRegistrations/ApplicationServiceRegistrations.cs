using SFA.DAS.AutoConfiguration;
using SFA.DAS.CommitmentsV2.Services.Shared;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Services;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        EmployerCommitmentsV2Configuration configuration)
    {
        services.AddTransient<IModelMapper, ModelMapper>();
        
        services.AddTransient<IAzureTableStorageConnectionAdapter, AzureTableStorageConnectionAdapter>();
        services.AddTransient<IEnvironmentService, EnvironmentService>();
        services.AddTransient<IAutoConfigurationService, TableStorageConfigurationService>();
        services.AddTransient<IDeliveryModelService, DeliveryModelService>();

        services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
        
        services.AddSingleton<ILinkGenerator, LinkGenerator>();
        services.AddSingleton<IUserAccountService, UserAccountService>();
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        
        services.AddSingleton(typeof(Interfaces.ICookieStorageService<>), typeof(Infrastructure.CookieService.CookieStorageService<>));
        services.AddSingleton<ICurrentDateTime, CurrentDateTime>();
        services.AddSingleton<ICreateCsvService, CreateCsvService>();
        services.AddSingleton<IAcademicYearDateProvider, AcademicYearDateProvider>();
        services.AddSingleton<IReservationsService, ReservationsService>();
        
        return services;
    }
}