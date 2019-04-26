using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public class AspNetStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDasCookiePolicy()
                .AddDasMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection()
                .UseDasHsts()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseMvc();
        }
    }
}