﻿using Microsoft.Extensions.Hosting;

namespace SFA.DAS.EmployerCommitmentsV2.Web.AppStart;

public static class HstsStartup
{
    public static IApplicationBuilder UseDasHsts(this IApplicationBuilder app)
    {
        var hostingEnvironment = app.ApplicationServices.GetService<IHostEnvironment>();

        if (!hostingEnvironment.IsDevelopment())
        {
            app.UseHsts();
        }

        return app;
    }
}