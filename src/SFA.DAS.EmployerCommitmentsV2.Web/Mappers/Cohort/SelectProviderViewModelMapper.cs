using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectProviderViewModelMapper : IMapper<SelectProviderRequest, SelectProviderViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public SelectProviderViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<SelectProviderViewModel> Map(SelectProviderRequest source)
    {
        var accountLegalEntity = await _commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        return new SelectProviderViewModel
        {
            AccountHashedId = source.AccountHashedId,
            CourseCode = source.CourseCode,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = accountLegalEntity.LegalEntityName,
            StartMonthYear = source.StartMonthYear,
            ReservationId = source.ReservationId,
            TransferSenderId = source.TransferSenderId,
            Origin = DetermineOrigin(source),
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId
        };

    }

    private static Origin DetermineOrigin(SelectProviderRequest source)
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