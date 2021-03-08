using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class CancelChangeOfProviderToChangeOfProviderRequestMapper : IMapper<CancelChangeOfProviderRequestViewModel, ChangeOfProviderRequest>
    {
        public Task<ChangeOfProviderRequest> Map(CancelChangeOfProviderRequestViewModel source)
        {
            var request = new ChangeOfProviderRequest {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderId = source.ProviderId,
                ProviderName = source.ProviderName,
                EmployerWillAdd = source.EmployerWillAdd,
                NewStartMonth = source.NewStartMonth,
                NewStartYear = source.NewStartYear,
                NewEndMonth = source.NewEndMonth,
                NewEndYear = source.NewEndYear,
                NewPrice = source.NewPrice,
                StoppedDuringCoP = source.StoppedDuringCoP
            };

            return Task.FromResult(request);
        }
    }
}
