using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EnterNewTrainingProviderViewModelMapper : IMapper<ChangeOfProviderRequest, EnterNewTrainingProviderViewModel>
{
    private readonly ICommitmentsApiClient _client;

    public EnterNewTrainingProviderViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<EnterNewTrainingProviderViewModel> Map(ChangeOfProviderRequest source)
    {
        var providersResponseTask = _client.GetAllProviders();
        var apprenticeshipTask = _client.GetApprenticeship(source.ApprenticeshipId.Value);

        await Task.WhenAll(providersResponseTask, apprenticeshipTask);

        var providersResponse = providersResponseTask.Result;
        var apprenticeship = apprenticeshipTask.Result;

        var result = new EnterNewTrainingProviderViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            Providers = providersResponse.Providers,
            CurrentProviderId = apprenticeship.ProviderId,
            ProviderId = source.ProviderId,
            NewStartMonth = source.NewStartMonth,
            NewStartYear = source.NewStartYear,
            NewEndMonth = source.NewEndMonth,
            NewEndYear = source.NewEndYear,
            NewPrice = source.NewPrice,
            Edit = source.Edit ?? false,
            StoppedDuringCoP = source.StoppedDuringCoP
        };

        return result;
    }
}