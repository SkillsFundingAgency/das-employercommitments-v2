using Microsoft.Extensions.Logging;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Home
{
    public class IndexViewModelMapper : IMapper<IndexRequest, IndexViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILogger<IndexViewModelMapper> _logger;
        private readonly IConfiguration _configuration;

        public IndexViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ILogger<IndexViewModelMapper> logger, IConfiguration configuration)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IndexViewModel> Map(IndexRequest source)
        {
            try
            {
                var response = await _commitmentsApiClient.GetProviderPaymentsPriority(source.AccountId, default(CancellationToken));
                return new IndexViewModel
                {
                    AccountHashedId = source.AccountHashedId,
                    ShowSetPaymentOrderLink = response.ProviderPaymentPriorities.Count() > 1,
                    PublicSectorReportingLink = $"https://reporting.{GetEnvironmentAndDomain(_configuration["ResourceEnvironmentName"])}/accounts/{source.AccountHashedId}/home"
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unable to map index view model");
                throw;
            }
        }
        
        private static string GetEnvironmentAndDomain(string environment)
        {
            if (environment.ToLower() == "local")
            {
                return "";
            }
            var environmentPart = environment.ToLower() == "prd" ? "manage-apprenticeships" : $"{environment.ToLower()}-eas.apprenticeships";
            var domainPart = environment.ToLower() == "prd" ?  "service" : "education";

            return $"{environmentPart}.{domainPart}.gov.uk";
        }
    }
}
