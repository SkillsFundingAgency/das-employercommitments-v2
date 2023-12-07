using System.Threading;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerAccounts.Api.Client;

namespace SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks;

public class EmployerAccountsApiHealthCheck : IHealthCheck
{
    private readonly IEmployerAccountsApiClient _employerAccountsApiClient;

    public EmployerAccountsApiHealthCheck(IEmployerAccountsApiClient employerAccountsApiClient)
    {
        _employerAccountsApiClient = employerAccountsApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _employerAccountsApiClient.Ping(cancellationToken);
                
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Degraded(exception.Message);
        }
    }
}