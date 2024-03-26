using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

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

    private static Origin DetermineOrigin(ConfirmProviderViewModel source)
    {
        if (source.ReservationId.HasValue)
        {
            return Origin.Reservations;
        }

        return !string.IsNullOrWhiteSpace(source.EncodedPledgeApplicationId) 
            ? Origin.LevyTransferMatching 
            : Origin.Apprentices;
    }
}