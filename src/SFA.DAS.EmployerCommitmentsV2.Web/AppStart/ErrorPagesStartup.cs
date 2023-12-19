using Microsoft.Extensions.Hosting;

namespace SFA.DAS.EmployerCommitmentsV2.Web.AppStart;

public static class ErrorPagesStartup
{
    public static IApplicationBuilder UseDasErrorPages(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseStatusCodePagesWithReExecute("/error", "?statuscode={0}");

        return app;
    }
}