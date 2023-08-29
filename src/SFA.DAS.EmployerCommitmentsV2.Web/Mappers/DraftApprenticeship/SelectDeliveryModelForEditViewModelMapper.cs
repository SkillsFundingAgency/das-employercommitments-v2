using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Encoding;
using DeliveryModel = SFA.DAS.CommitmentsV2.Types.DeliveryModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class SelectDeliveryModelForEditViewModelMapper : IMapper<EditDraftApprenticeshipViewModel, SelectDeliveryModelForEditViewModel>
    {
        private readonly IApprovalsApiClient _apiClient;
        private readonly IEncodingService _encodingService;

        public SelectDeliveryModelForEditViewModelMapper(IApprovalsApiClient apiClient, IEncodingService encodingService)
        {
             _apiClient = apiClient;
            _encodingService = encodingService;
        }

        public async Task<SelectDeliveryModelForEditViewModel> Map(EditDraftApprenticeshipViewModel source)
        {
            var draftApprenticeshipId = _encodingService.Decode(source.DraftApprenticeshipHashedId, EncodingType.ApprenticeshipId);
            var apiResponse = await _apiClient.GetEditDraftApprenticeshipSelectDeliveryModel(source.ProviderId, (long)source.CohortId, draftApprenticeshipId, source.CourseCode);

            return new SelectDeliveryModelForEditViewModel
            {
                DeliveryModel = apiResponse.DeliveryModel,
                DeliveryModels = apiResponse.DeliveryModels,
                LegalEntityName = apiResponse.EmployerName,
                CourseCode = source.CourseCode,
                HasUnavailableFlexiJobAgencyDeliveryModel = apiResponse.HasUnavailableDeliveryModel && source.DeliveryModel == DeliveryModel.FlexiJobAgency,
                ShowFlexiJobAgencyDeliveryModelConfirmation = apiResponse.HasUnavailableDeliveryModel &&
                                                              source.DeliveryModel == DeliveryModel.FlexiJobAgency &&
                                                              apiResponse.DeliveryModels.Count == 1
            };
        }
    }
}
