using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class ConfirmProviderViewModelMapper : IMapper<ConfirmProviderRequest, ConfirmProviderViewModel>
    {
        public ConfirmProviderViewModel Map(ConfirmProviderRequest source)
        {
            return new ConfirmProviderViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
                CourseCode = source.CourseCode,
                ProviderId = source.ProviderId
            };
        }
    }
}