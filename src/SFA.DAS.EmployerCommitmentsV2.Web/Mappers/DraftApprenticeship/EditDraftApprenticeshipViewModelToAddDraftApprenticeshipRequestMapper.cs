using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class EditDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper : IMapper<EditDraftApprenticeshipViewModel, AddDraftApprenticeshipRequest>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IEncodingService _encodingService;

    public EditDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
        => (_commitmentsApiClient, _encodingService) = (commitmentsApiClient, encodingService);

    public async Task<AddDraftApprenticeshipRequest> Map(EditDraftApprenticeshipViewModel source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId.Value);

        return new AddDraftApprenticeshipRequest
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityId = cohort.AccountLegalEntityId,
            AccountLegalEntityHashedId = _encodingService.Encode(cohort.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
            CohortId = source.CohortId.HasValue ? source.CohortId.Value : 0,
            CohortReference = source.CohortReference,
            DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
            ReservationId = source.ReservationId.HasValue ? source.ReservationId.Value : System.Guid.Empty,
            CourseCode = source.CourseCode,
            ProviderId = source.ProviderId,
            DeliveryModel = source.DeliveryModel,
            Cost = source.Cost,
            EmploymentPrice = source.EmploymentPrice,
            EmploymentEndDate = source.EmploymentEndDate.Date,
            CacheKey = source.CacheKey
        };
    }
}