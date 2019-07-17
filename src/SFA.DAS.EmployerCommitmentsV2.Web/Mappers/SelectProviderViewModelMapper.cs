using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class SelectProviderViewModelMapper : IMapper<SelectProviderRequest, SelectProviderViewModel>
    {
        public SelectProviderViewModel Map(SelectProviderRequest source)
        {
            return new SelectProviderViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CourseCode = source.CourseCode,
                EmployerAccountLegalEntityPublicHashedId = source.EmployerAccountLegalEntityPublicHashedId,
                StartMonthYear = source.StartMonthYear,
                ReservationId = source.ReservationId,
            };
        }
    }
}
