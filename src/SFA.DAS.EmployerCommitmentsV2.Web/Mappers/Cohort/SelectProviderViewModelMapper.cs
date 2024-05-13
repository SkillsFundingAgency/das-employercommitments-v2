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
        var providersResponseTask = _commitmentsApiClient.GetAllProviders();
        var accountLegalEntityTask = _commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        await Task.WhenAll(providersResponseTask, accountLegalEntityTask);

        var providersResponse = providersResponseTask.Result;
        var accountLegalEntity = accountLegalEntityTask.Result;

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
            Providers = providersResponse.Providers
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