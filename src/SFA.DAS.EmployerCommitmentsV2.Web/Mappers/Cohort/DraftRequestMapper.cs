using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class DraftRequestMapper : IMapper<DraftRequest, DraftViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public DraftRequestMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<DraftViewModel> Map(DraftRequest source)
        {
            var request = new GetCohortsRequest {AccountId = source.AccountId};
            var result = await _commitmentsApiClient.GetCohorts(request);

            return new DraftViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountId = source.AccountId,
                Cohorts = result.Cohorts
            };

        }
    }
}
