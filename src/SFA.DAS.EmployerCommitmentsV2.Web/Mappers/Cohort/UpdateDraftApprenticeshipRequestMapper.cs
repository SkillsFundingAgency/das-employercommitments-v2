using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class UpdateDraftApprenticeshipRequestMapper : IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipRequest>
    {
        public Task<UpdateDraftApprenticeshipRequest> Map(EditDraftApprenticeshipViewModel source)
        {
            return Task.FromResult(new UpdateDraftApprenticeshipRequest
            {
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                CourseOption = source.CourseOption,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                Reference = source.Reference
            });
        }
    }
}
