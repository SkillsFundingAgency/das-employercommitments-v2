using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class WhoWillEnterTheDetailsToSendNewTrainingProviderRequestMapper : IMapper<WhoWillEnterTheDetailsViewModel, SendNewTrainingProviderRequest>
    {
        public Task<SendNewTrainingProviderRequest> Map(WhoWillEnterTheDetailsViewModel source)
        {
            var result = new SendNewTrainingProviderRequest
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderId = source.ProviderId
            };

            return Task.FromResult(result);
        }
    }
}
