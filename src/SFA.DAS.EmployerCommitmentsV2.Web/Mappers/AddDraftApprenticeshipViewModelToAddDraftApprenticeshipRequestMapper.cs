using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using AddDraftApprenticeshipRequest = SFA.DAS.CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper : IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipRequest>
    {
        public Task<AddDraftApprenticeshipRequest> Map(AddDraftApprenticeshipViewModel source)
        {
            return Task.FromResult(new AddDraftApprenticeshipRequest
            {
                UserId = "X", // TODO: Remove this from the request as it's not required
                ProviderId = 1, // TODO: Remove this from the request as it's not required
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                OriginatorReference = source.Reference,
                ReservationId = source.ReservationId
            });
        }
    }
}