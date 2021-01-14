using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Types;

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
            var getProviderTask = _client.GetProvider(source.ProviderId);
            var getApprenticeshipTask = _client.GetApprenticeship(source.ApprenticeshipId);

            await Task.WhenAll(getProviderTask, getApprenticeshipTask);

            var result = new ChangeProviderRequestedConfirmationViewModel
            {
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId,
                ProviderName = getProviderTask.Result.Name,
                ApprenticeName = $"{getApprenticeshipTask.Result.FirstName} {getApprenticeshipTask.Result.LastName}",
                ProviderAddDetails = source.ProviderAddDetails.GetValueOrDefault(),
            };

            return result;
        }
    }
}
