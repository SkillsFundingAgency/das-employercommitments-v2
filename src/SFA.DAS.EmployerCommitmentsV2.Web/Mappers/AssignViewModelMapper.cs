using System.Threading.Tasks;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class AssignViewModelMapper : IMapper<AssignRequest, AssignViewModel> {
        public Task<AssignViewModel> Map(AssignRequest source)
        {
            return Task.FromResult(new AssignViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
                CourseCode = source.CourseCode,
                ProviderId = source.ProviderId
            });
        }
    }
}