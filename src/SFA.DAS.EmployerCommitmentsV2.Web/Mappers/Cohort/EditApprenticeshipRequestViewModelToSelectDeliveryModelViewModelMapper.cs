using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class EditApprenticeshipRequestViewModelToSelectDeliveryModelViewModelMapper : IMapper<EditApprenticeshipRequestViewModel, SelectDeliveryModelViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IApprovalsApiClient _approvalsApiClient;

        public EditApprenticeshipRequestViewModelToSelectDeliveryModelViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient)
            => (_commitmentsApiClient, _approvalsApiClient) = (commitmentsApiClient, approvalsApiClient);

        public async Task<SelectDeliveryModelViewModel> Map(EditApprenticeshipRequestViewModel source)
        {
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);
            var cohort = await _commitmentsApiClient.GetCohort(apprenticeship.CohortId);

            var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(cohort.ProviderId.HasValue ? cohort.ProviderId.Value : 0, source.CourseCode);

            return new SelectDeliveryModelViewModel
            {
                DeliveryModel = source.DeliveryModel,
                DeliveryModels = response.DeliveryModels.ToArray()
            };
        }
    }
}
