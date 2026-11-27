using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using SFA.DAS.EmployerCommitmentsV2.Web.Logging;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Logging;

public class OpenTelemetryLogFiltersTests
{
    [Test]
    public void Then_Missing_Antiforgery_Cookie_Logs_Are_Not_Exported_As_Information()
    {
        var rules = RulesForOpenTelemetry();

        rules.Should().ContainSingle(rule =>
            rule.CategoryName == OpenTelemetryLogFilters.AntiforgeryValidationCategory &&
            rule.LogLevel == LogLevel.Warning);
    }

    [Test]
    public void Then_Other_Microsoft_Information_Logs_Are_Still_Exported()
    {
        var rules = RulesForOpenTelemetry();

        rules.Should().ContainSingle(rule =>
            rule.CategoryName == "Microsoft" &&
            rule.LogLevel == LogLevel.Information);
    }

    private static IReadOnlyList<LoggerFilterRule> RulesForOpenTelemetry()
    {
        var services = new ServiceCollection();
        services.AddLogging(OpenTelemetryLogFilters.Apply);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<LoggerFilterOptions>>().Value;
        var providerName = typeof(OpenTelemetryLoggerProvider).FullName;

        return options.Rules.Where(rule => rule.ProviderName == providerName).ToList();
    }
}
