using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution;
using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public class AspNetStartup
    {
        private readonly IConfiguration _configuration;

        public AspNetStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDasCookiePolicy()
                .AddDasHealthChecks()
                .AddDasMvc()
                .AddDasEmployerAuthentication(_configuration)
                .AddDasAuthorization();
                .AddMemoryCache();
        }

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDasErrorPages()
                .UseUnauthorizedAccessExceptionHandler()
                .UseHttpsRedirection()
                .UseDasHsts()
                .UseStaticFiles()
                .UseDasHealthChecks()
                .UseAuthentication()
                .UseCookiePolicy()
                .UseMvc()
                ;
        }
    }
}