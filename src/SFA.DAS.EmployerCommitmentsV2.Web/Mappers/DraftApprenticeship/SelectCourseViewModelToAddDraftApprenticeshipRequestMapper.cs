using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class SelectCourseViewModelToAddDraftApprenticeshipRequestMapper : IMapper<SelectCourseViewModel, AddDraftApprenticeshipRequest>
    {
        public Task<AddDraftApprenticeshipRequest> Map(SelectCourseViewModel source)
        {
            return Task.FromResult(new AddDraftApprenticeshipRequest
            {
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                AccountHashedId = source.AccountHashedId,
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
