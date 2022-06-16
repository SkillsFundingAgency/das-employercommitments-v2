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

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper : IMapper<AddDraftApprenticeshipRequest, SelectDeliveryModelViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IApprovalsApiClient _approvalsApiClient;
        private readonly IFjaaAgencyService _fjaaAgencyService;
        private readonly IAuthorizationService _authorizationService;

        public AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient, IFjaaAgencyService fjaaAgencyService, IAuthorizationService authorizationService)
            => (_commitmentsApiClient, _approvalsApiClient, _fjaaAgencyService, _authorizationService) = (commitmentsApiClient, approvalsApiClient, fjaaAgencyService, authorizationService);

        public async Task<SelectDeliveryModelViewModel> Map(AddDraftApprenticeshipRequest source)
        {
            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

            var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(cohort.ProviderId.HasValue ? cohort.ProviderId.Value : 0, source.CourseCode);
            var deliveryModels = response.DeliveryModels.ToList();

            if (_authorizationService.IsAuthorized(EmployerFeature.FJAA))
            {
                bool agencyExists = await _fjaaAgencyService.AgencyExists((int)source.AccountLegalEntityId);
                deliveryModels = await _fjaaAgencyService.AssignDeliveryModels(deliveryModels, agencyExists);
            }

            return new SelectDeliveryModelViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CohortId = source.CohortId,
                CohortReference = source.CohortReference,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
                DeliveryModels = deliveryModels.ToArray(),
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
            };
        }
    }
}
