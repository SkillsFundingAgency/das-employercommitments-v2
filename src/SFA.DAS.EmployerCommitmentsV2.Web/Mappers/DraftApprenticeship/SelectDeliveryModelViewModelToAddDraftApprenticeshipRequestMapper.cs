using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper : IMapper<SelectDeliveryModelViewModel, AddDraftApprenticeshipRequest>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;

        public SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
            => (_commitmentsApiClient, _encodingService) = (commitmentsApiClient, encodingService);

        public async Task<AddDraftApprenticeshipRequest> Map(SelectDeliveryModelViewModel source)
        {
            var cohortId = _encodingService.Decode(source.CohortReference, EncodingType.CohortReference);
            var cohort = await _commitmentsApiClient.GetCohort(cohortId);

            return new AddDraftApprenticeshipRequest
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CohortId = source.CohortId,
                CohortReference = source.CohortReference,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
                ProviderId = cohort.ProviderId.Value,
                ReservationId = source.ReservationId.HasValue ? source.ReservationId.Value : System.Guid.Empty,
                StartMonthYear = source.StartMonthYear,
                ShowTrainingDetails = source.ShowTrainingDetails
            };
        }
    }
}
