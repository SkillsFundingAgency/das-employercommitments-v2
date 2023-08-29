using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper : IMapper<AddDraftApprenticeshipRequest, SelectDeliveryModelViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IApprovalsApiClient _approvalsApiClient;
        private readonly IEncodingService _encodingService;

        public AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _approvalsApiClient = approvalsApiClient;
            _encodingService = encodingService;
        }

        public async Task<SelectDeliveryModelViewModel> Map(AddDraftApprenticeshipRequest source)
        {
            var cohortId = _encodingService.Decode(source.CohortReference, EncodingType.CohortReference);
            var accountLegalEntityId = _encodingService.Decode(source.AccountLegalEntityHashedId, EncodingType.AccountLegalEntityId);
            var cohort = await _commitmentsApiClient.GetCohort(cohortId);

            var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(cohort.ProviderId.HasValue ? cohort.ProviderId.Value : 0, source.CourseCode, accountLegalEntityId);

            return new SelectDeliveryModelViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityId = accountLegalEntityId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CohortId = cohortId,
                CohortReference = source.CohortReference,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
                DeliveryModels = response.DeliveryModels.ToArray(),
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
            };
        }
    }
}
