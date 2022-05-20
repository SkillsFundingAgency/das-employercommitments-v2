using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipRequestToEditDraftApprenticeshipViewModelMapper : IMapper<AddDraftApprenticeshipRequest, EditDraftApprenticeshipViewModel>
    {
        public Task<EditDraftApprenticeshipViewModel> Map(AddDraftApprenticeshipRequest source)
        {
            return Task.FromResult(new EditDraftApprenticeshipViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ProviderId = source.ProviderId,
                CohortId = source.CohortId,
                CohortReference = source.CohortReference,
                DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
                ReservationId = source.ReservationId,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
            });
        }
    }
}
