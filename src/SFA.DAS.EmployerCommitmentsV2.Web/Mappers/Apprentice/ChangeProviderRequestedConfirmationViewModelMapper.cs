using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeProviderRequestedConfirmationViewModelMapper : IMapper<ChangeProviderRequestedConfirmationRequest, ChangeProviderRequestedConfirmationViewModel>
    {
        private readonly ICommitmentsApiClient _client;
        private readonly IEncodingService _encodingService;

        public ChangeProviderRequestedConfirmationViewModelMapper(ICommitmentsApiClient client, IEncodingService encodingService)
        {
            _client = client;
            _encodingService = encodingService;
        }

        public async Task<ChangeProviderRequestedConfirmationViewModel> Map(ChangeProviderRequestedConfirmationRequest source)
        {
            
            var apprenticeshipId = _encodingService.Decode(source.ApprenticeshipHashedId, EncodingType.ApprenticeshipId);
            
            var providerResponse = await _client.GetProvider(source.ProviderId);
            var apprenticeshipResponse = await _client.GetApprenticeship(apprenticeshipId);

            var result = new ChangeProviderRequestedConfirmationViewModel
            {
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId, 
                Provider = providerResponse,
                ApprenticeName = $"{apprenticeshipResponse.FirstName} {apprenticeshipResponse.LastName}"
            };
            
            return result;
        }
    }
}
