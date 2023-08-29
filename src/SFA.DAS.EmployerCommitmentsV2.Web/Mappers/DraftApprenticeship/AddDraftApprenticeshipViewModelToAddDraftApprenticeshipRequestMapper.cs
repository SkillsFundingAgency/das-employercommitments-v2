using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using AddDraftApprenticeshipRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship.AddDraftApprenticeshipRequest;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper : IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipRequest>
    {
        public Task<AddDraftApprenticeshipRequest> Map(AddDraftApprenticeshipViewModel source)
        {
            return Task.FromResult(new AddDraftApprenticeshipRequest
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                ProviderId = source.ProviderId,
                DeliveryModel = source.DeliveryModel,
                CohortReference = source.CohortReference,                
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                EmploymentPrice = source.EmploymentPrice,
                EmploymentEndDate = source.EmploymentEndDate.Date,
                ReservationId = source.ReservationId.HasValue ? source.ReservationId.Value : System.Guid.Empty,
            });
        }
    }
}
