using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectProviderViewModelMapper : IMapper<SelectProviderRequest, SelectProviderViewModel>
{
    private readonly IApprovalsApiClient _outerApiClient;

    public SelectProviderViewModelMapper(IApprovalsApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    public async Task<SelectProviderViewModel> Map(SelectProviderRequest source)
    {
        var selectProviderDetails = await _outerApiClient.GetSelectProviderDetails(source.AccountId, source.AccountLegalEntityId);

        var providers = selectProviderDetails.Providers.ToList();
        var accountLegalEntity = selectProviderDetails.AccountLegalEntity;

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
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            Providers = providers
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