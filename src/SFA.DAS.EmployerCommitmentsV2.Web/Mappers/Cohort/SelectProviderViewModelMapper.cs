using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class SelectProviderViewModelMapper : IMapper<SelectProviderRequest, SelectProviderViewModel>
    {
        public Task<SelectProviderViewModel> Map(SelectProviderRequest source)
        {
            var result = new SelectProviderViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CourseCode = source.CourseCode,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                StartMonthYear = source.StartMonthYear,
                ReservationId = source.ReservationId,
                TransferSenderId = source.TransferSenderId,
                Origin = source.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices
            };

            return Task.FromResult(result);
        }
    }
}
