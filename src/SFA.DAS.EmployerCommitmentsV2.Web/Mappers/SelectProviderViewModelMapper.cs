using System.Threading.Tasks;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class SelectProviderViewModelMapper : 
        IMapper<ConfirmProviderViewModel, SelectProviderViewModel>, 
        IMapper<SelectProviderRequest, SelectProviderViewModel>
    {
        public Task<SelectProviderViewModel> Map(ConfirmProviderViewModel source)
        {
            return Task.FromResult(new SelectProviderViewModel
            {
                ProviderId = source.ProviderId.ToString(),
                ReservationId = source.ReservationId,
                CourseCode = source.CourseCode,
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                StartMonthYear = source.StartMonthYear
            });
        }

        public Task<SelectProviderViewModel> Map(SelectProviderRequest source)
        {
            return Task.FromResult(new SelectProviderViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CourseCode = source.CourseCode,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                StartMonthYear = source.StartMonthYear,
                ReservationId = source.ReservationId,
            });
        }
    }
}