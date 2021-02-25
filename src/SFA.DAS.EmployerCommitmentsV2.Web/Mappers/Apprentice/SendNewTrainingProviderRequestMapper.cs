using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class SendNewTrainingProviderRequestMapper : IMapper<EnterNewTrainingProviderViewModel, SendNewTrainingProviderRequest>
    {
        public Task<SendNewTrainingProviderRequest> Map(EnterNewTrainingProviderViewModel source)
        {
            var result = new SendNewTrainingProviderRequest
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderId = source.ProviderId.Value,
                StoppedDuringCoP = source.StoppedDuringCoP
            };

            return Task.FromResult(result);
        }
    }
}
