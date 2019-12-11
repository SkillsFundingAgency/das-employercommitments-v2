using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Home
{
    public class IndexViewModelMapper : IMapper<IndexRequest, IndexViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public IndexViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<IndexViewModel> Map(IndexRequest source)
        {
            var providers = await _commitmentsApiClient.GetApprovedProviders(source.AccountId, default(CancellationToken));
            return new IndexViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ShowSetPaymentOrderLink = providers.ProviderIds.Count() > 1
            };
        }
    }
}
