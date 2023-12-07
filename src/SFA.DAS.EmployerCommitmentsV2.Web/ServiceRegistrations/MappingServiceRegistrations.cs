using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class MappingServiceRegistrations
{
    public static IServiceCollection AddModelMappings(this IServiceCollection services)
    {
        var mappingAssembly = typeof(ChangeVersionViewModelMapper).Assembly;

        var mappingTypes = mappingAssembly
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>)));

        foreach (var mapperType in mappingTypes.Where(x => x != typeof(AttachUserInfoToSaveRequests<,>)))
        {
            var mapperInterface = mapperType
                .GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>));

            services.AddTransient(mapperInterface, mapperType);
        }

        services.Decorate(typeof(IMapper<,>), typeof(AttachUserInfoToSaveRequests<,>));

        return services;
    }
}