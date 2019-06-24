using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class AddDraftApprenticeshipRequestMapper : IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipRequest>
    {
        public AddDraftApprenticeshipRequest Map(AddDraftApprenticeshipViewModel source)
        {
            return new AddDraftApprenticeshipRequest
            {
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                OriginatorReference = source.Reference
            };
        }
    }
}
