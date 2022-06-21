using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class EditApprenticeshipRequestViewModelToSelectDeliveryModelViewModelMapper : IMapper<EditApprenticeshipRequestViewModel, SelectDeliveryModelViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IApprovalsApiClient _approvalsApiClient;
        private readonly IAuthorizationService _authorizationService;

        public EditApprenticeshipRequestViewModelToSelectDeliveryModelViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient, IAuthorizationService authorizationService)
            => (_commitmentsApiClient, _approvalsApiClient, _authorizationService) = (commitmentsApiClient, approvalsApiClient, authorizationService);

        public async Task<SelectDeliveryModelViewModel> Map(EditApprenticeshipRequestViewModel source)
        {
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);
            var cohort = await _commitmentsApiClient.GetCohort(apprenticeship.CohortId);

            long accountLegalEntityId = 0;

            if (_authorizationService.IsAuthorized(EmployerFeature.FJAA))
            {
                accountLegalEntityId = cohort.AccountLegalEntityId;
            }

            var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(cohort.ProviderId.HasValue ? cohort.ProviderId.Value : 0, source.CourseCode, accountLegalEntityId);

            return new SelectDeliveryModelViewModel
            {
                DeliveryModel = source.DeliveryModel,
                DeliveryModels = response.DeliveryModels.ToArray()
            };
        }
    }
}
