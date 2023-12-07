using AspNetCore.IServiceCollection.AddIUrlHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;
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
        services
            .AddApplicationServices()
            .AddCommitmentsApiClient(_configuration)
            .AddDasAuthorization()
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