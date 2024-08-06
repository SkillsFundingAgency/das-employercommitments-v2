using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class EditDraftApprenticeshipViewModelToEditDraftApprenticeshipRequestMapper : IMapper<EditDraftApprenticeshipViewModel, EditDraftApprenticeshipRequest>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IEncodingService _encodingService;

    public EditDraftApprenticeshipViewModelToEditDraftApprenticeshipRequestMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
        => (_commitmentsApiClient, _encodingService) = (commitmentsApiClient, encodingService);

    public async Task<EditDraftApprenticeshipRequest> Map(EditDraftApprenticeshipViewModel source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId.Value);

        return new EditDraftApprenticeshipRequest
        {
            AccountHashedId = source.AccountHashedId,
            //AccountLegalEntityId = cohort.AccountLegalEntityId,
            AccountLegalEntityHashedId = _encodingService.Encode(cohort.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
            //CohortId = source.CohortId.HasValue ? source.CohortId.Value : 0,
            CohortReference = source.CohortReference,
            DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
            CourseCode = source.CourseCode,
            DeliveryModel = source.DeliveryModel,
        };
    }
}