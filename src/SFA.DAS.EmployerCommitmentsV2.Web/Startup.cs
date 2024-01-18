using AspNetCore.IServiceCollection.AddIUrlHelper;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.AppStart;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;
using SFA.DAS.EmployerUrlHelper.DependencyResolution;
using SFA.DAS.GovUK.Auth.AppStart;

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
        services.AddHttpClient();
        services.AddLogging(builder =>
        {
            builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
        });
        
        services.AddConfigurationOptions(_configuration);
        services.AddEncodingServices(_configuration);
        services.AddModelMappings();
        services.AddApprovalsApiClient();
            
        var employerCommitmentsV2Configuration = _configuration.Get<EmployerCommitmentsV2Configuration>();
        
        services
            .AddApplicationServices(employerCommitmentsV2Configuration)
            .AddCommitmentsApiClient(_configuration)
            .AddAccountsApiClient(employerCommitmentsV2Configuration)
            .AddCommitmentsAuthorization()
            .AddAuthenticationServices()
            .AddCommitmentPermissionsApiClient()
            .AddDasHealthChecks()
            .AddDasMaMenuConfiguration(_configuration)
            .AddDasMvc()
            .AddUrlHelper()
            .AddEmployerUrlHelper()
            .AddMemoryCache()
            .AddApplicationInsightsTelemetry()
            .AddDasDataProtection(_configuration, _environment);
       
        if (_configuration.UseGovUkSignIn())
        {
            var govConfig = _configuration.GetSection("SFA.DAS.Employer.GovSignIn");
            govConfig["ResourceEnvironmentName"] = _configuration["ResourceEnvironmentName"];
            govConfig["StubAuth"] = _configuration["StubAuth"];
            services.AddAndConfigureGovUkAuthentication(govConfig,
                typeof(EmployerAccountPostAuthenticationClaimsHandler),
                "",
                "/service/SignIn-Stub");
        }
        else
        {
            services.AddAndConfigureEmployerAuthentication(_configuration);
        }
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
            .UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute()
            );
    }
}