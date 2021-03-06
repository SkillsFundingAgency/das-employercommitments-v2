﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class ContentSecurityPolicyStartup
    {
        public static IApplicationBuilder UseDasContentSecurityPolicy(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();
                if (!hostingEnvironment.IsDevelopment())
                {
                    context.Response.Headers["Content-Security-Policy"] =
                        "default-src 'self' 'unsafe-inline' https://*.zdassets.com https://*.zendesk.com wss://*.zendesk.com wss://*.zopim.com; " +
                        "style-src 'self' 'unsafe-inline' https://*.azureedge.net; " +
                        "img-src 'self' https://*.azureedge.net *.google-analytics.com https://*.zdassets.com https://*.zendesk.com wss://*.zendesk.com wss://*.zopim.com; " +
                        "script-src 'self' 'unsafe-inline' " +
                        "https://das-prd-frnt-end.azureedge.net https://das-demo-frnt-end.azureedge.net https://das-pp-frnt-end.azureedge.net https://das-test-frnt-end.azureedge.net https://das-at-frnt-end.azureedge.net " +
                        "*.googletagmanager.com *.postcodeanywhere.co.uk *.google-analytics.com *.googleapis.com https://*.zdassets.com https://*.zendesk.com wss://*.zendesk.com wss://*.zopim.com; " +
                        "font-src 'self' data:;";
                }

                await next();
            });

            return app;
        }
    }
}