using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Linq;
using SFA.DAS.Encoding;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ReviewRequestViewModelMapper : IMapper<GetCohortsResponse, ReviewViewModel>
    {
        private readonly IEncodingService _encodingService;

        public ReviewRequestViewModelMapper(IEncodingService encodingSummary)
        {
            _encodingService = encodingSummary;
        }

        public Task<ReviewViewModel> Map(GetCohortsResponse source)
        {
            var response = new ReviewViewModel();
            
            response.CohortSummary = source.Cohorts
                .Where(x => x.WithParty == Party.Employer && !x.IsDraft)
                .OrderBy(z => z.CreatedOn)
                .Select(y => new ReviewCohortSummaryViewModel
                {
                    CohortReference = _encodingService.Encode(y.CohortId, EncodingType.CohortReference),
                    ProviderName = y.ProviderName,
                    NumberOfApprentices = y.NumberOfDraftApprentices,
                    LastMessage = GetMessage(y.LatestMessageFromProvider)
                }).ToList();

            return Task.FromResult(response);
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