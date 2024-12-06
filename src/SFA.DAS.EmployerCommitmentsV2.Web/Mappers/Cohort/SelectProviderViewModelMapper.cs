using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectProviderViewModelMapper : IMapper<OG_CacheModel, SelectProviderViewModel>
{
    private readonly IApprovalsApiClient _outerApiClient;

    public SelectProviderViewModelMapper(IApprovalsApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    public async Task<SelectProviderViewModel> Map(OG_CacheModel source)
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
            Providers = providers,
            OG_CacheKey = source.CacheKey
        };
    }

    private static Origin DetermineOrigin(OG_CacheModel source)
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