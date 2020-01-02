using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.Encoding;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ReviewRequestViewModelMapper : IMapper<CohortsByAccountRequest, ReviewViewModel>
    {
        private readonly IEncodingService _encodingService;
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public ReviewRequestViewModelMapper(ICommitmentsApiClient commitmentApiClient, IEncodingService encodingSummary)
        {
            _encodingService = encodingSummary;
            _commitmentsApiClient = commitmentApiClient;
        }

        public async Task<ReviewViewModel> Map(CohortsByAccountRequest source)
        {
            var cohortsResponse = await _commitmentsApiClient.GetCohorts(new GetCohortsRequest { AccountId = source.AccountId });

            var reviewViewModel = new ReviewViewModel
            {
                AccountHashedId = source.AccountHashedId,
                Cohorts = cohortsResponse.Cohorts
                    .Where(x => x.GetStatus() == CohortStatus.Review)
                    .OrderBy(z => z.CreatedOn)
                    .Select(y => new ReviewCohortSummaryViewModel
                    {
                        CohortReference = _encodingService.Encode(y.CohortId, EncodingType.CohortReference),
                        ProviderName = y.ProviderName,
                        NumberOfApprentices = y.NumberOfDraftApprentices,
                        LastMessage = GetMessage(y.LatestMessageFromProvider)
                    }).ToList()
            };

            return reviewViewModel;
        }

        private string GetMessage(Message latestMessageFromProvider)
        {
            if (!string.IsNullOrWhiteSpace(latestMessageFromProvider?.Text))
            {
                return latestMessageFromProvider.Text;
            }

            return "No message added";
        }
    }
}