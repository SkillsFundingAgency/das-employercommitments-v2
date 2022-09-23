using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.ProviderCommitments.Web.Mappers.DraftApprenticeship
{
    public class SelectDeliveryModelForEditViewModelMapper : IMapper<EditDraftApprenticeshipViewModel, SelectDeliveryModelForEditViewModel>
    {
        private readonly IApprovalsApiClient _apiClient;

        public SelectDeliveryModelForEditViewModelMapper(IApprovalsApiClient apiClient)
        {
             _apiClient = apiClient;
        }

        public async Task<SelectDeliveryModelForEditViewModel> Map(EditDraftApprenticeshipViewModel source)
        {
            var apiResponse = await _apiClient.GetEditDraftApprenticeshipSelectDeliveryModel(source.ProviderId, (long)source.CohortId, source.DraftApprenticeshipId, source.CourseCode);

            return new SelectDeliveryModelForEditViewModel
            {
                DeliveryModel = apiResponse.DeliveryModel,
                DeliveryModels = apiResponse.DeliveryModels,
                LegalEntityName = apiResponse.EmployerName,
                CourseCode = source.CourseCode
            };
        }
    }
}
