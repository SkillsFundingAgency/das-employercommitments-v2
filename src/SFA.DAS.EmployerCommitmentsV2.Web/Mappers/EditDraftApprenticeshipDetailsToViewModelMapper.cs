using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class EditDraftApprenticeshipDetailsToViewModelMapper : IMapper<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel>
    {
        public EditDraftApprenticeshipViewModel Map(EditDraftApprenticeshipDetails source) =>
            new EditDraftApprenticeshipViewModel(source.DateOfBirth, source.StartDate, source.EndDate)
            {
                DraftApprenticeshipId = source.DraftApprenticeshipId,
                DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
                CohortId = source.CohortId,
                CohortReference = source.CohortReference,
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Uln = source.UniqueLearnerNumber,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                Reference = source.OriginatorReference
            };
    }
}