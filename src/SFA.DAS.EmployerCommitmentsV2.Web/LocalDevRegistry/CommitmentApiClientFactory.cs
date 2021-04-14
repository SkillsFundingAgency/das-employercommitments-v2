﻿using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.CommitmentsV2.Api.Client.Http;
using SFA.DAS.Http;
using System;

namespace SFA.DAS.CommitmentsV2.Api.Client
{
    public class LocalDevApiClientFactory : ICommitmentsApiClientFactory, ICommitmentPermissionsApiClientFactory
    {
        private readonly CommitmentsClientApiConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public LocalDevApiClientFactory(CommitmentsClientApiConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public ICommitmentsApiClient CreateClient()
        {
            var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (value == "Development")
            {
                var httpClientBuilder = new HttpClientBuilder();
                var httpClient = httpClientBuilder
               .WithDefaultHeaders()
               .Build();

                httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);

                var restHttpClient = new CommitmentsRestHttpClient(httpClient, _loggerFactory);
                return new CommitmentsApiClient(restHttpClient);
            }
            else
            {
                throw new UnauthorizedAccessException("Not accessible");
            }
        }

        ICommitmentPermissionsApiClient ICommitmentPermissionsApiClientFactory.CreateClient()
        {
            var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (value == "Development")
            {
                var httpClientBuilder = new HttpClientBuilder();
                var httpClient = httpClientBuilder
               .WithDefaultHeaders()
               .Build();

                httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);

                var restHttpClient = new CommitmentsRestHttpClient(httpClient, _loggerFactory);
                return new CommitmentPermissionsApiClient(restHttpClient);
            }
            else
            {
                throw new UnauthorizedAccessException("Not accessible");
            }
        }
    }
}
