using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class CohortsSummaryViewModelMapper : IMapper<CohortsByAccountRequest, CohortsViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILinkGenerator _linkGenerator;

        public CohortsSummaryViewModelMapper(ICommitmentsApiClient commitmentApiClient, ILinkGenerator linkGenerator)
        {
            _commitmentsApiClient = commitmentApiClient;
            _linkGenerator = linkGenerator;
        }

        public async Task<CohortsViewModel> Map(CohortsByAccountRequest source)
        {
            var cohorts = (await _commitmentsApiClient.GetCohorts(new GetCohortsRequest { AccountId = source.AccountId })).Cohorts;

            return new CohortsViewModel
            {
                CohortsInDraft = new CohortCardLinkViewModel(
                    cohorts.Count(x => x.GetStatus() == CohortStatus.Draft),
                    "drafts", 
                    _linkGenerator.CommitmentsV2Link($"{source.AccountHashedId}/unapproved/draft")),
                CohortsInReview = new CohortCardLinkViewModel(
                    cohorts.Count(x => x.GetStatus() == CohortStatus.Review),
                    "ready to review", 
                    _linkGenerator.CommitmentsV2Link($"{source.AccountHashedId}/unapproved/review")),
                CohortsWithTrainingProvider = new CohortCardLinkViewModel(
                    cohorts.Count(x => x.GetStatus() == CohortStatus.WithProvider), 
                    "with providers",
                    _linkGenerator.CommitmentsV2Link($"{source.AccountHashedId}/unapproved/with-training-provider")),
                CohortsWithTransferSender = new CohortCardLinkViewModel(
                    cohorts.Count(x => x.GetStatus() == CohortStatus.WithTransferSender),
                    "with transfer sending employers",
                    _linkGenerator.CommitmentsV2Link($"{source.AccountHashedId}/unapproved/with-transfer-sender")),
                
                BackLink = _linkGenerator.CommitmentsV2Link($"{source.AccountHashedId}/unapproved")
            };
        }
    }
}