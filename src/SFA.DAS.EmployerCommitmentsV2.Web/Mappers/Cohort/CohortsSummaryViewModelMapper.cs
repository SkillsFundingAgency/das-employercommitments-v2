using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class CohortsSummaryViewModelMapper : IMapper<CohortsByAccountRequest, CohortsViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IUrlHelper _urlHelper;

        public CohortsSummaryViewModelMapper(ICommitmentsApiClient commitmentApiClient, IUrlHelper urlHelper)
        {
            _commitmentsApiClient = commitmentApiClient;
            _urlHelper = urlHelper;
        }

        public async Task<CohortsViewModel> Map(CohortsByAccountRequest source)
        {
            var cohorts = (await _commitmentsApiClient.GetCohorts(new GetCohortsRequest { AccountId = source.AccountId })).Cohorts;

            return new CohortsViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortsInDraft = new CohortCardLinkViewModel(
                    cohorts.Count(x => x.GetStatus() == CohortStatus.Draft),
                    "drafts", 
                    _urlHelper.Action("Draft", "Cohort", new { source.AccountHashedId }),
                    CohortStatus.Draft.ToString()),
                CohortsInReview = new CohortCardLinkViewModel(
                    cohorts.Count(x => x.GetStatus() == CohortStatus.Review),
                    "ready to review", 
                    _urlHelper.Action("Review", "Cohort", new { source.AccountHashedId }),
                     CohortStatus.Review.ToString()),
                CohortsWithTrainingProvider = new CohortCardLinkViewModel(
                    cohorts.Count(x => x.GetStatus() == CohortStatus.WithProvider),
                    "with training providers",
                    _urlHelper.Action("WithTrainingProvider", "Cohort", new { source.AccountHashedId }),
                    CohortStatus.WithProvider.ToString()),
                CohortsWithTransferSender = new CohortCardLinkViewModel(
                    cohorts.Count(x => x.GetStatus() == CohortStatus.WithTransferSender),
                    "with transfer sending employers",
                    _urlHelper.Action("WithTransferSender", "Cohort", new { source.AccountHashedId }),
                    CohortStatus.WithTransferSender.ToString())
            };
        }
    }
}