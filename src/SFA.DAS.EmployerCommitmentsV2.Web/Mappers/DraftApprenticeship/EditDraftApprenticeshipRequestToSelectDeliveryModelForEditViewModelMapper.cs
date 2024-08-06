using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class EditDraftApprenticeshipRequestToSelectDeliveryModelForEditViewModelMapper : IMapper<EditDraftApprenticeshipRequest, SelectDeliveryModelForEditViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IApprovalsApiClient _approvalsApiClient;

    public EditDraftApprenticeshipRequestToSelectDeliveryModelForEditViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient)
        => (_commitmentsApiClient, _approvalsApiClient) = (commitmentsApiClient, approvalsApiClient);

    public async Task<SelectDeliveryModelForEditViewModel> Map(EditDraftApprenticeshipRequest source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

        var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(cohort.ProviderId.HasValue ? cohort.ProviderId.Value : 0, source.CourseCode, source.AccountLegalEntityId);

        return new SelectDeliveryModelForEditViewModel
        {
            AccountHashedId = source.AccountHashedId,
            CourseCode = source.CourseCode,
            DeliveryModel = (DeliveryModel?)source.DeliveryModel,
            DeliveryModels = response.DeliveryModels.Select(x=> (DeliveryModel)x).ToList(),
        };
    }
}