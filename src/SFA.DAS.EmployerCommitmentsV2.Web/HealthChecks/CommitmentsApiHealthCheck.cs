using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Extensions;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks
{
    public class CommitmentsApiHealthCheck : IHealthCheck
    {
        private readonly ICommitmentsApiClient _apiClient;
        private readonly ILogger<CommitmentsApiHealthCheck> _logger;

        public CommitmentsApiHealthCheck(ICommitmentsApiClient apiClient, ILogger<CommitmentsApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            _logger.LogInformation($"Started '{context.Registration.Name}'");

            try
            {
                var stopwatch = Stopwatch.StartNew();

                await _apiClient.HealthCheck();

                stopwatch.Stop();

                var elapsed = stopwatch.Elapsed.ToHealthCheckString();

                _logger.LogInformation($"Finished '{context.Registration.Name}' in '{elapsed}'");

                return HealthCheckResult.Healthy(null, new Dictionary<string, object> { { "elapsed", elapsed } });
            }
            catch (RestHttpClientException ex)
            {
                _logger.LogError($"Failed '{context.Registration.Name}': {nameof(ex.StatusCode)}='{ex.StatusCode}', {nameof(ex.ReasonPhrase)}='{ex.ReasonPhrase}'");

                return HealthCheckResult.Unhealthy(null, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed '{context.Registration.Name}'", ex);

                return HealthCheckResult.Unhealthy(null, ex);
            }
        }
    }
}