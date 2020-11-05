using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
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
            var changeOfProviderRequest = changeOfProviderRequestsTask.Result;
            var priceHistory = priceHistoryTask.Result;

            var request = changeOfProviderRequest.ChangeOfPartyRequests.FirstOrDefault(r => r.Status == ChangeOfPartyRequestStatus.Pending);

            var cohort = await _client.GetCohort(request.CohortId.Value);
            
            var newProvider = await _client.GetProvider(request.ProviderId.Value);

            var result = new ViewChangesViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
                CurrentProviderName = apprenticeship.ProviderName,
                CurrentStartDate = apprenticeship.StartDate,
                CurrentEndDate = apprenticeship.EndDate,
                CurrentPrice = (int)GetCurrentPrice(priceHistory),
                NewProviderName = newProvider.Name,
                NewStartDate = request.StartDate,
                NewEndDate = request.EndDate,
                NewPrice = request.Price,
                CurrentParty = cohort.WithParty,
                CohortReference = _encodingService.Encode(request.CohortId.Value, EncodingType.CohortReference)

            };

            return result;
        }

        private decimal GetCurrentPrice(GetPriceEpisodesResponse priceHistory)
        {
            var currentPriceDetails = priceHistory.PriceEpisodes.FirstOrDefault(p => p.ToDate == null);

            return currentPriceDetails.Cost;
        }
    }
}
