using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EnterNewTrainingProviderViewModelMapper : IMapper<EnterNewTrainingProviderRequest, EnterNewTrainingProviderViewModel>
    {
        private readonly ICommitmentsApiClient _client;
        private readonly IEncodingService _encodingService;
        public EnterNewTrainingProviderViewModelMapper(ICommitmentsApiClient client, IEncodingService encodingService)
        {
            _client = client;
            _encodingService = encodingService;
        }

        public async Task<EnterNewTrainingProviderViewModel> Map(EnterNewTrainingProviderRequest source)
        {
            var accountId = _encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
           

            var providers = await _client.GetApprovedProviders(accountId, CancellationToken.None);

            var result = new EnterNewTrainingProviderViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId
            };

            return result;
        }
    }
}
