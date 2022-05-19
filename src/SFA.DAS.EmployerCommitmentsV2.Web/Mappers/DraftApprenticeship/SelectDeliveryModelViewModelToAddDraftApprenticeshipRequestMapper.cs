using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper : IMapper<SelectDeliveryModelViewModel, AddDraftApprenticeshipRequest>
    {
        public Task<AddDraftApprenticeshipRequest> Map(SelectDeliveryModelViewModel source)
        {
            return Task.FromResult(new AddDraftApprenticeshipRequest
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CohortId = source.CohortId,
                CohortReference = source.CohortReference,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId.HasValue ? source.ReservationId.Value : System.Guid.Empty,
                StartMonthYear = source.StartMonthYear,
            });
        }
    }
}
