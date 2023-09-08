using AspNetCore.IServiceCollection.AddIUrlHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution;
using SFA.DAS.EmployerUrlHelper.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public class AspNetStartup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public AspNetStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
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

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry, _configuration);
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
}