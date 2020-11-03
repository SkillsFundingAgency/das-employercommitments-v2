using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeProviderRequestedConfirmationViewModelMapper : IMapper<ChangeProviderRequestedConfirmationRequest, ChangeProviderRequestedConfirmationViewModel>
    {
        private readonly ICommitmentsApiClient _client;

        public ChangeProviderRequestedConfirmationViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<ChangeProviderRequestedConfirmationViewModel> Map(ChangeProviderRequestedConfirmationRequest source)
        {
            var providerResponse = await _client.GetProvider(source.ProviderId);
            var apprenticeshipResponse = await _client.GetApprenticeship(source.ApprenticeshipId);

            var result = new ChangeProviderRequestedConfirmationViewModel
            {
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId, 
                ProviderName = providerResponse.Name,
                ApprenticeName = $"{apprenticeshipResponse.FirstName} {apprenticeshipResponse.LastName}"
            };
            
            return result;
        }
    }
}
