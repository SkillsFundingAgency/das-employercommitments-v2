using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class SelectOptionViewModelToUpdateDraftApprenticeshipRequestMapper : IMapper<SelectOptionViewModel, UpdateDraftApprenticeshipRequest>
    {
        public Task<UpdateDraftApprenticeshipRequest> Map(SelectOptionViewModel source)
        {
            return Task.FromResult(new UpdateDraftApprenticeshipRequest
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                CourseOption = source.CourseOption == "N/A" ? string.Empty : source.CourseOption,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                Reference = source.Reference,
                ReservationId = source.ReservationId
            });
        }
    }
}
