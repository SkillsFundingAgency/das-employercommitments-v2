using System.Threading.Tasks;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class UpdateDraftApprenticeshipRequestMapper : IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipRequest>
    {
        public Task<UpdateDraftApprenticeshipRequest> Map(EditDraftApprenticeshipViewModel source) =>
            Task.FromResult(new UpdateDraftApprenticeshipRequest
            {
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                Reference = source.Reference
            });
    }
}
