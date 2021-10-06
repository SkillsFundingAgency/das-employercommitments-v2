using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class SelectProviderViewModelFromConfirmProviderMapper : IMapper<ConfirmProviderViewModel, SelectProviderViewModel>
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
                StartMonthYear = source.StartMonthYear,
                TransferSenderId = source.TransferSenderId,
                Origin = DetermineOrigin(source),
                EncodedPledgeApplicationId = source.EncodedPledgeApplicationId
            });
        }

        private Origin DetermineOrigin(ConfirmProviderViewModel source)
        {
            if (source.ReservationId.HasValue)
            {
                return Origin.Reservations;
            }

            if (!string.IsNullOrWhiteSpace(source.EncodedPledgeApplicationId))
            {
                return Origin.LevyTransferMatching;
            }

            return Origin.Apprentices;
        }
    }
}