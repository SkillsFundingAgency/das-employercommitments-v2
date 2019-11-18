using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class DraftRequestMapper : IMapper<DraftRequest, DraftViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILinkGenerator _linkGenerator;

        public DraftRequestMapper(ICommitmentsApiClient commitmentsApiClient, ILinkGenerator linkGenerator)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _linkGenerator = linkGenerator;
        }

        public async Task<DraftViewModel> Map(DraftRequest source)
        {
            var request = new GetCohortsRequest {AccountId = source.AccountId};
            var apiResponse = await _commitmentsApiClient.GetCohorts(request);
            var cohorts = apiResponse.Cohorts.Where(x => x.WithParty == Party.Employer && x.IsDraft);

            return new DraftViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountId = source.AccountId,
                Cohorts = cohorts,
                BackLink = _linkGenerator.Cohorts(source.AccountHashedId)
            };

        }
    }
}
