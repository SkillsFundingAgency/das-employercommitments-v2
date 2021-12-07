using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.Http;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution
{
    public class ApprovalsApiClientFactory : IApprovalsApiClientFactory
    {
        private readonly ApprovalsApiClientConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public ApprovalsApiClientFactory(ApprovalsApiClientConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public IApprovalsApiClient CreateClient()
        {
            var httpClient = new HttpClientBuilder()
                                    .WithDefaultHeaders()
                                    .WithApimAuthorisationHeader(_configuration)
                                    .WithLogging(_loggerFactory)
                                    .Build();

            if (!_configuration.ApiBaseUrl.EndsWith("/"))
                httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl + "/");
            else
                httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);

            //var httpClientFactory = new ManagedIdentityHttpClientFactory(_configuration, _loggerFactory);
            //var httpClient = httpClientFactory.CreateHttpClient();
            var apiClient = new ApprovalsApiClient(new RestHttpClient(httpClient));

            return apiClient;
        }
    }
}
