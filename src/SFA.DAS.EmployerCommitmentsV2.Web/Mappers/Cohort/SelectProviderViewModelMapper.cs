using System;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;

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
                Origin = source.Origin
            };

            return Task.FromResult(result);
        }
    }
}
