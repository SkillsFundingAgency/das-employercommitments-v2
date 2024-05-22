using SFA.DAS.CommitmentsV2.Api.Client;
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
        var providersResponseTask = _outerApiClient.GetAllProviders();
        var accountLegalEntityTask = _outerApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        await Task.WhenAll(providersResponseTask, accountLegalEntityTask);

        var providersResponse = await providersResponseTask;
        var accountLegalEntity = await accountLegalEntityTask;

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
            Providers = providersResponse.Providers.ToList()
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