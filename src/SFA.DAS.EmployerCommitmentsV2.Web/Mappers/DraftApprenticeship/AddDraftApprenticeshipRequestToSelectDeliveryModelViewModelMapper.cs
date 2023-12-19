using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper : IMapper<AddDraftApprenticeshipRequest, SelectDeliveryModelViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IApprovalsApiClient _approvalsApiClient;

    public AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient)
        => (_commitmentsApiClient, _approvalsApiClient) = (commitmentsApiClient, approvalsApiClient);

    public async Task<SelectDeliveryModelViewModel> Map(AddDraftApprenticeshipRequest source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

        var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(cohort.ProviderId.HasValue ? cohort.ProviderId.Value : 0, source.CourseCode, source.AccountLegalEntityId);

        return new SelectDeliveryModelViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            CohortId = source.CohortId,
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