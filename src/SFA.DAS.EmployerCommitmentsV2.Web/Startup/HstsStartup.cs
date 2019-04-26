using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class HstsStartup
    {
        public static IApplicationBuilder UseDasHsts(this IApplicationBuilder app)
        {
            var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();

            if (!hostingEnvironment.IsDevelopment())
            {
                app.UseHsts();
            }

            return app;
        }
    }
}