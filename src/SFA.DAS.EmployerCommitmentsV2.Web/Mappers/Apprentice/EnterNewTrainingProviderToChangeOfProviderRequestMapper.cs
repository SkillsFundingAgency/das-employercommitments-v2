using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EnterNewTrainingProviderToChangeOfProviderRequestMapper : IMapper<EnterNewTrainingProviderViewModel, ChangeOfProviderRequest>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public EnterNewTrainingProviderToChangeOfProviderRequestMapper(ICommitmentsApiClient client)
        {
            _commitmentsApiClient = client;
        }
        public async Task<ChangeOfProviderRequest> Map(EnterNewTrainingProviderViewModel source)
        {
            var provider = await _commitmentsApiClient.GetProvider(source.ProviderId.Value, CancellationToken.None);
            
            return new ChangeOfProviderRequest
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderId = source.ProviderId,
                ProviderName = provider.Name,
                EmployerWillAdd = source.EmployerWillAdd,
                NewStartMonth = source.NewStartMonth,
                NewStartYear = source.NewStartYear,
                NewEndMonth = source.NewEndMonth,
                NewEndYear = source.NewEndYear,
                NewPrice = source.NewPrice,
                StoppedDuringCoP = source.StoppedDuringCoP
            };
        }
    }
}
