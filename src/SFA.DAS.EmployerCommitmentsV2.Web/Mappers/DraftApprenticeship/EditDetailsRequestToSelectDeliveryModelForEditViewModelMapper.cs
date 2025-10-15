using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using DeliveryModel = SFA.DAS.CommitmentsV2.Types.DeliveryModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class EditDetailsRequestToSelectDeliveryModelForEditViewModelMapper : IMapper<EditDetailsRequest, SelectDeliveryModelForEditViewModel>
{
    private readonly IApprovalsApiClient _apiClient;

    public EditDetailsRequestToSelectDeliveryModelForEditViewModelMapper(IApprovalsApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<SelectDeliveryModelForEditViewModel> Map(EditDetailsRequest source)
    {
        var cohort = await _apiClient.GetCohortDetails(source.AccountId, source.CohortId);
        var apiResponse = await _apiClient.GetEditDraftApprenticeshipSelectDeliveryModel(cohort.ProviderId.Value, source.CohortId, source.DraftApprenticeshipId, source.CourseCode);

        return new SelectDeliveryModelForEditViewModel
        {
            DeliveryModel = apiResponse.DeliveryModel,
            DeliveryModels = apiResponse.DeliveryModels,
            LegalEntityName = apiResponse.EmployerName,
            CourseCode = source.CourseCode,
            HasUnavailableFlexiJobAgencyDeliveryModel = apiResponse.HasUnavailableDeliveryModel && source.DeliveryModel == DeliveryModel.FlexiJobAgency,
            ShowFlexiJobAgencyDeliveryModelConfirmation = apiResponse.HasUnavailableDeliveryModel &&
                                                          source.DeliveryModel == DeliveryModel.FlexiJobAgency &&
                                                          apiResponse.DeliveryModels.Count == 1,
            CacheKey = source.CacheKey
        };
    }
}