using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class ErrorPagesStartup
    {
        public static IApplicationBuilder UseDasErrorPages(this IApplicationBuilder app)
        {
            var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();

            if (hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error.html");
            }

            return app.UseStatusCodePagesWithReExecute("/{0}.html");
        }
    }
}