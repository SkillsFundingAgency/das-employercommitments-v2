using AspNetCore.IServiceCollection.AddIUrlHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Logs;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.AppStart;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;
using SFA.DAS.EmployerUrlHelper.DependencyResolution;

namespace SFA.DAS.EmployerCommitmentsV2.Web;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration.BuildDasConfiguration();
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_configuration);
        services.AddHttpClient();
        
        services.AddLogging(builder =>
        {
            builder.AddFilter<OpenTelemetryLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<OpenTelemetryLoggerProvider>("Microsoft", LogLevel.Information);
        });

        services.AddConfigurationOptions(_configuration);
        services.AddEncodingServices(_configuration);
        services.AddModelMappings();
        services.AddApprovalsApiClient();

        var employerCommitmentsV2Configuration = _configuration.Get<EmployerCommitmentsV2Configuration>();

        services
            .AddDasEmployerAuthentication(_configuration)
            .AddApplicationServices()
            .AddCommitmentsApiClient(_configuration)
            .AddAccountsApiClient(employerCommitmentsV2Configuration)
            .AddAuthorizationServices(_configuration)
            .AddCommitmentPermissionsApiClient()
            .AddDasHealthChecks()
            .AddDasMaMenuConfiguration(_configuration)
            .AddDasMvc()
            .AddUrlHelper()
            .AddEmployerUrlHelper()
            .AddMemoryCache()
            .AddCache(_environment, _configuration)
            .AddOpenTelemetryRegistration(_configuration)
            .AddDasDataProtection(_configuration, _environment);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDasErrorPages(_environment)
            .UseHttpsRedirection()
            .UseDasHsts()
            .UseStaticFiles()
            .UseDasHealthChecks()
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization()
            .UseMiddleware<MissingAddApprenticeshipCacheKeyMiddleware>()
            .UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute()
            );
    }
}