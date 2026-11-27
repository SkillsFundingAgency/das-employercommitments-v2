using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Logging;

public static class OpenTelemetryLogFilters
{
    public const string AntiforgeryValidationCategory =
        "Microsoft.AspNetCore.Mvc.ViewFeatures.Filters.ValidateAntiforgeryTokenAuthorizationFilter";

    public static void Apply(ILoggingBuilder builder)
    {
        builder.AddFilter<OpenTelemetryLoggerProvider>(string.Empty, LogLevel.Information);
        builder.AddFilter<OpenTelemetryLoggerProvider>("Microsoft", LogLevel.Information);
        builder.AddFilter<OpenTelemetryLoggerProvider>(AntiforgeryValidationCategory, LogLevel.Warning);
    }
}
