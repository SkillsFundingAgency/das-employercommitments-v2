using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class SelectProviderRequestMapper : IMapper<SelectProviderViewModel, SelectProviderRequest>
    {
        public Task<SelectProviderRequest> Map(SelectProviderViewModel source)
        {
            return Task.FromResult(new SelectProviderRequest
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CourseCode = source.CourseCode,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear
            });
        }
    }
}
