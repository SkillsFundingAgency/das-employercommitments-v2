using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

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
                Ukprn = source.Ukprn
            };

            return Task.FromResult(result);
        }
    }
}
