using System.Threading.Tasks;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ApprovedViewModelMapper : IMapper<ApprovedRequest, ApprovedViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly IAuthorizationService _authorizationService;

        public ApprovedViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService, IAuthorizationService authorizationService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _authorizationService = authorizationService;
        }

        public async Task<ApprovedViewModel> Map(ApprovedRequest source)
        {
            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

            return new ApprovedViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference,
                WithParty = cohort.WithParty,
                AccountLegalEntityHashedId = _encodingService.Encode(cohort.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                Message = cohort.LatestMessageCreatedByEmployer,
                HasApprenticeEmail = await _authorizationService.IsAuthorizedAsync(EmployerFeature.ApprenticeEmail)
            };
        }
    }
}
