using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectProviderViewModelMapper(IApprovalsApiClient outerApiClient, ILogger<SelectProviderViewModelMapper> logger) : IMapper<AddApprenticeshipCacheModel, SelectProviderViewModel>
{
    public async Task<SelectProviderViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var selectProviderDetails = await outerApiClient.GetSelectProviderDetails(source.AccountId, source.AccountLegalEntityId);

        var providers = selectProviderDetails.Providers.ToList();
        var accountLegalEntity = selectProviderDetails.AccountLegalEntity;
        
        logger.LogInformation("SelectProviderViewModelMapper source: {Data}", JsonSerializer.Serialize(source));
        
        return new SelectProviderViewModel
        {
            AccountHashedId = source.AccountHashedId,
            CourseCode = source.CourseCode,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = accountLegalEntity.LegalEntityName,
            StartMonthYear = source.StartMonthYear,
            ReservationId = source.ReservationId,
            Origin = DetermineOrigin(source),
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            Providers = providers,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey
        };
    }

    private static Origin DetermineOrigin(AddApprenticeshipCacheModel source)
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