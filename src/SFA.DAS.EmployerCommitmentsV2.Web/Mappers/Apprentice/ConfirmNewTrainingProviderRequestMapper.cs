using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ConfirmNewTrainingProviderRequestMapper : IMapper<EnterNewTrainingProviderViewModel, ConfirmNewTrainingProviderRequest>
    {
        public Task<ConfirmNewTrainingProviderRequest> Map(EnterNewTrainingProviderViewModel source)
        {

            var result = new ConfirmNewTrainingProviderRequest
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                Ukprn = source.Ukprn
            };

            return Task.FromResult(result);
        }
    }
}
