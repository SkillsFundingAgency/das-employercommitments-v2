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
                app.UseExceptionHandler("/error-local-development")
                   .UseStatusCodePagesWithReExecute("/error-local-development", "?statuscode={0}");
            }
            else
            {
                app.UseExceptionHandler("/error")
                   .UseStatusCodePagesWithReExecute("/error", "?statuscode={0}");
            }

            return app;
        }
    }
}