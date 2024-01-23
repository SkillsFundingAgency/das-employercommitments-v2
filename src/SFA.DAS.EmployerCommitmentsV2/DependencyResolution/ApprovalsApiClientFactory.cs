using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApprovalsApiClientFactory(ApprovalsApiClientConfiguration configuration, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            _httpContextAccessor = httpContextAccessor;
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

            var apiClient = new ApprovalsApiClient(new OuterApiClient(httpClient, _configuration, _loggerFactory.CreateLogger<OuterApiClient>(), _httpContextAccessor));
            return apiClient;
        }
    }
}
