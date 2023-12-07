using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ViewChangesViewModelMapper : IMapper<ViewChangesRequest, ViewChangesViewModel>
{
    private readonly ICommitmentsApiClient _client;
    private readonly IEncodingService _encodingService;

    public ViewChangesViewModelMapper(ICommitmentsApiClient client, IEncodingService encodingService)
    {
        _client = client;
        _encodingService = encodingService;
    }

    public async Task<ViewChangesViewModel> Map(ViewChangesRequest source)
    {
        var apprenticeshipTask = _client.GetApprenticeship(source.ApprenticeshipId);
        var changeOfProviderRequestsTask = _client.GetChangeOfPartyRequests(source.ApprenticeshipId);
        var priceHistoryTask = _client.GetPriceEpisodes(source.ApprenticeshipId);

        await Task.WhenAll(apprenticeshipTask, changeOfProviderRequestsTask, priceHistoryTask);

        var apprenticeship = apprenticeshipTask.Result;
        var changeOfProviderRequests = changeOfProviderRequestsTask.Result;
        var priceHistory = priceHistoryTask.Result;

        var pendingChangeRequest = changeOfProviderRequests.ChangeOfPartyRequests.FirstOrDefault(r => r.Status == ChangeOfPartyRequestStatus.Pending && r.ChangeOfPartyType == ChangeOfPartyRequestType.ChangeProvider);

        var newProvider = await _client.GetProvider(pendingChangeRequest.ProviderId.Value);

        var result = new ViewChangesViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
            CurrentProviderName = apprenticeship.ProviderName,
            CurrentStartDate = apprenticeship.StartDate.Value,
            CurrentEndDate = apprenticeship.EndDate,
            CurrentPrice = priceHistory.PriceEpisodes.GetPrice(),
            NewProviderName = newProvider.Name,
            NewStartDate = pendingChangeRequest.StartDate,
            NewEndDate = pendingChangeRequest.EndDate,
            NewPrice = pendingChangeRequest.Price,
            CurrentParty = pendingChangeRequest.WithParty.Value,
            CohortReference = _encodingService.Encode(pendingChangeRequest.CohortId.Value, EncodingType.CohortReference)
        };

        return result;
    }
}