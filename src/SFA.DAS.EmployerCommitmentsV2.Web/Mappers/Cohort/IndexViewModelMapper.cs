using System.Threading.Tasks;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class IndexViewModelMapper : IMapper<IndexRequest, IndexViewModel>
    {
        public Task<IndexViewModel> Map(IndexRequest source)
        {
            return Task.FromResult(new IndexViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
                CourseCode = source.CourseCode
            });
        }
    }
}