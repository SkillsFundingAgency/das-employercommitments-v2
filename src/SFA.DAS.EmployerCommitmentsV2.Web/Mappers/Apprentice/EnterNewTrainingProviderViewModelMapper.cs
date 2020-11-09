using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EnterNewTrainingProviderViewModelMapper : IMapper<EnterNewTrainingProviderRequest, EnterNewTrainingProviderViewModel>
    {
        private readonly ICommitmentsApiClient _client;

        public EnterNewTrainingProviderViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<EnterNewTrainingProviderViewModel> Map(EnterNewTrainingProviderRequest source)
        {
            var providersResponse = await _client.GetAllProviders();
            
            var result = new EnterNewTrainingProviderViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                Providers = providersResponse.Providers
            };

            return result;
        }
    }
}
