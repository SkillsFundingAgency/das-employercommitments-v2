using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper : IMapper<SelectDeliveryModelViewModel, AddDraftApprenticeshipRequest>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper(ICommitmentsApiClient commitmentsApiClient)
            => _commitmentsApiClient = commitmentsApiClient;

        public async Task<AddDraftApprenticeshipRequest> Map(SelectDeliveryModelViewModel source)
        {
            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

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
            };
        }
    }
}
