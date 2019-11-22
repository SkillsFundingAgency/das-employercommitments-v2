using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ReviewRequestViewModelMapper : IMapper<ReviewRequest, ReviewViewModel>
    {
        private readonly IEncodingService _encodingService;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
//        private readonly ILinkGenerator _linkGenerator;

        public ReviewRequestViewModelMapper(ICommitmentsApiClient commitmentApiClient, IEncodingService encodingSummary)
        {
            _encodingService = encodingSummary;
            _commitmentsApiClient = commitmentApiClient;
        }

        public async Task<ReviewViewModel> Map(ReviewRequest source)
        {
            var cohortsResponse = await _commitmentsApiClient.GetCohorts(new GetCohortsRequest { AccountId = source.AccountId });

            var reviewViewModel =  new ReviewViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortSummary = cohortsResponse.Cohorts
                .Where(x => x.WithParty == Party.Employer && !x.IsDraft)
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