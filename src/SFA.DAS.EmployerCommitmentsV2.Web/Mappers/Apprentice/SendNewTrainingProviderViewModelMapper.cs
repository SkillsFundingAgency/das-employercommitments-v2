using Microsoft.Extensions.Logging;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class SendNewTrainingProviderViewModelMapper : IMapper<SendNewTrainingProviderRequest, SendNewTrainingProviderViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentApiClient;
        private readonly ILogger<SendNewTrainingProviderViewModelMapper> _logger;

        public SendNewTrainingProviderViewModelMapper(ICommitmentsApiClient commitmentsApiClient,  ILogger<SendNewTrainingProviderViewModelMapper> logger)
        {
            _commitmentApiClient = commitmentsApiClient;
            _logger = logger;
        }

        public async Task<SendNewTrainingProviderViewModel> Map(SendNewTrainingProviderRequest source)
        {
            try
            {
                var data = await GetApprenticeshipData(source.ApprenticeshipId, source.ProviderId);

                var result = new SendNewTrainingProviderViewModel
                {
                    ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                    AccountHashedId = source.AccountHashedId,
                    EmployerName = data.Apprenticeship.EmployerName,
                    ApprenticeName = $"{data.Apprenticeship.FirstName} {data.Apprenticeship.LastName}",
                    OldProviderName = data.Apprenticeship.ProviderName,
                    NewProviderName = data.TrainingProvider.Name,
                    ProviderId = source.ProviderId,
                    ApprenticeshipStatus = data.Apprenticeship.Status,
                    StoppedDuringCoP = source.StoppedDuringCoP
                };

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error mapping apprenticeshipId {source.ApprenticeshipId} to model {nameof(SendNewTrainingProviderViewModel)}", e);
                throw;
            }
        }

        private async Task<(GetApprenticeshipResponse Apprenticeship, GetProviderResponse TrainingProvider)> 
            GetApprenticeshipData(long apprenticeshipId, long providerId)
        {
            var apprenticeshipTask =  _commitmentApiClient.GetApprenticeship(apprenticeshipId);
            var trainingProviderTask = _commitmentApiClient.GetProvider(providerId);

            await Task.WhenAll(apprenticeshipTask, trainingProviderTask);

            var apprenticeship = await apprenticeshipTask;
            var trainingProvider = await trainingProviderTask;

            return (apprenticeship, trainingProvider);
        }
    }
}
