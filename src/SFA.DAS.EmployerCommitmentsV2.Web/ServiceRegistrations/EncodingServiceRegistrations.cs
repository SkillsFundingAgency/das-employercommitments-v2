using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class EncodingServiceRegistrations
{
    public static IServiceCollection AddEncodingServices(this IServiceCollection services, IConfiguration configuration) 
    {
        var encodingConfigJson = configuration.GetSection(ConfigurationKeys.Encoding).Value;
        var encodingConfig = JsonConvert.DeserializeObject<EncodingConfig>(encodingConfigJson);
        services.AddSingleton(encodingConfig);

        services.AddSingleton<IEncodingService, EncodingService>();

        return services;
    }
}