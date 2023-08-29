using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using System.Threading.Tasks;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipRequestToEditDraftApprenticeshipViewModelMapper : IMapper<AddDraftApprenticeshipRequest, EditDraftApprenticeshipViewModel>
    {
        private readonly IEncodingService _encodingService;

        public AddDraftApprenticeshipRequestToEditDraftApprenticeshipViewModelMapper(IEncodingService encodingService)
        {
            _encodingService = encodingService;
        }

        public Task<EditDraftApprenticeshipViewModel> Map(AddDraftApprenticeshipRequest source)
        {
            var cohortId = _encodingService.Decode(source.CohortReference, EncodingType.CohortReference);
            return Task.FromResult(new EditDraftApprenticeshipViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ProviderId = source.ProviderId,
                CohortId = cohortId,
                CohortReference = source.CohortReference,
                DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
                ReservationId = source.ReservationId,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
            });
        }
    }
}
