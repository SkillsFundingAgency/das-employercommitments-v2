using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class ErrorPagesStartup
    {
        public static IApplicationBuilder UseDasErrorPages(this IApplicationBuilder app, IHostingEnvironment environment)
        {

            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithRedirects("~/error/?statuscode={0}");
            }

            return app;
        }
    }
}