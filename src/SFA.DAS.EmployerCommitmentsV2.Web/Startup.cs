using AspNetCore.IServiceCollection.AddIUrlHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.AppStart;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;
using SFA.DAS.EmployerUrlHelper.DependencyResolution;

namespace SFA.DAS.EmployerCommitmentsV2.Web;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration.BuildDasConfiguration();
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_configuration);
        services.AddLogging(builder =>
        {
            builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
        });
        
        services.AddHttpContextAccessor();
        services.AddConfigurationOptions(_configuration);
        services.AddEncodingServices(_configuration);
        services.AddModelMappings();
        services.AddApprovalsApiClient();
            
        var employerCommitmentsV2Configuration = _configuration.Get<EmployerCommitmentsV2Configuration>();
        
        services
            .AddApplicationServices(employerCommitmentsV2Configuration)
            .AddCommitmentsApiClient(_configuration)
            .AddAccountsApiClient(employerCommitmentsV2Configuration)
            .AddAuthorizationServices()
            .AddDasEmployerAuthentication(_configuration)
            .AddDasHealthChecks()
            .AddDasMaMenuConfiguration(_configuration)
            .AddDasMvc(_configuration)
            .AddUrlHelper()
            .AddEmployerUrlHelper()
            .AddMemoryCache()
            .AddApplicationInsightsTelemetry()
            .AddDataProtection(_configuration, _environment);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDasErrorPages(_environment)
            .UseUnauthorizedAccessExceptionHandler()
            .UseHttpsRedirection()
            .UseDasHsts()
            .UseStaticFiles()
            .UseDasHealthChecks()
            .UseAuthentication()
            .UseMvc();
    }
}