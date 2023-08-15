using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using System.Collections.Generic;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetApprenticeshipUpdatesResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfPartyRequestsResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfProviderChainResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses
{
    public class GetManageApprenticeshipDetailsResponse
    {
        public GetApprenticeshipResponse Apprenticeship { get; set; }

        public IEnumerable<GetPriceEpisodesResponse.PriceEpisode> PriceEpisodes { get; set; }
        public IEnumerable<ApprenticeshipUpdate> ApprenticeshipUpdates { get; set; }
        public IReadOnlyCollection<DataLock> DataLocks { get; set; }
        public IReadOnlyCollection<ChangeOfPartyRequest> ChangeOfPartyRequests { get; set; }
        public IReadOnlyCollection<ChangeOfProviderLink> ChangeOfProviderChain { get; set; }
        public IReadOnlyCollection<ApprenticeshipOverlappingTrainingDateRequest> OverlappingTrainingDateRequest { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
    }
}