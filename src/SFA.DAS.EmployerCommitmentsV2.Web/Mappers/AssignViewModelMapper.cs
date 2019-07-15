using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class AssignViewModelMapper : IMapper<AssignRequest, AssignViewModel>
    {
        public AssignViewModel Map(AssignRequest request)
        {
            return new AssignViewModel
            {
                AccountHashedId = request.AccountHashedId,
                EmployerAccountLegalEntityPublicHashedId = request.EmployerAccountLegalEntityPublicHashedId,
                ReservationId = request.ReservationId,
                StartMonthYear = request.StartMonthYear,
                CourseCode = request.CourseCode
            };
        }
    }
}