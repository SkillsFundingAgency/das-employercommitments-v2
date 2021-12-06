using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching;
using SFA.DAS.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution
{
    public class LevyTransferMatchingApiClientFactory : ILevyTransferMatchingApiClientFactory
    {
        private readonly LevyTransferMatchingApiClientConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public LevyTransferMatchingApiClientFactory(LevyTransferMatchingApiClientConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public ILevyTransferMatchingApiClient CreateClient()
        {
            var httpClientFactory = new ManagedIdentityHttpClientFactory(_configuration, _loggerFactory);
            var httpClient = httpClientFactory.CreateHttpClient();
            var apiClient = new LevyTransferMatchingApiClient(new RestHttpClient(httpClient));

            return apiClient;
        }
    }
}
