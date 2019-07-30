using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks
{
    public class CommitmentsApiHealthCheck : IHealthCheck
    {        
        private readonly ICommitmentsApiClient _apiClient;

        public CommitmentsApiHealthCheck(ICommitmentsApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _apiClient.WhoAmI();
            var data = new Dictionary<string, object> { ["Roles"] = response.Roles };
            
            return response.Roles.Contains(Role.Employer) && !response.Roles.Contains(Role.Provider)
                ? HealthCheckResult.Healthy(null, data)
                : HealthCheckResult.Unhealthy($"Client should be in '{Role.Employer}' role and not be in '{Role.Provider}' role", null, data);
        }
    }
}