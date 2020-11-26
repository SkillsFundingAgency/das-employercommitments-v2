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
            var providersResponseTask = _client.GetAllProviders();
            var apprenticeshipTask = _client.GetApprenticeship(source.ApprenticeshipId);

            await Task.WhenAll(providersResponseTask, apprenticeshipTask);

            var providersResponse = providersResponseTask.Result;
            var apprenticeship = apprenticeshipTask.Result;

            var result = new EnterNewTrainingProviderViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                Providers = providersResponse.Providers,
                CurrentProviderId = apprenticeship.ProviderId
            };

            return result;
        }
    }
}
