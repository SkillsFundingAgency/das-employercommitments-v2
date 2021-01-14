using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class WhatIsTheNewStartDateToChangeOfProviderRequest : IMapper<WhatIsTheNewStartDateViewModel, ChangeOfProviderRequest>
    {
        public Task<ChangeOfProviderRequest> Map(WhatIsTheNewStartDateViewModel source)
        {
            var result = new ChangeOfProviderRequest
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderId = source.ProviderId,
                ProviderName = source.ProviderName,
                EmployerWillAdd = source.EmployerWillAdd,
                NewStartMonth = source.NewStartMonth,
                NewStartYear = source.NewStartYear,
                NewEndMonth = source.NewEndMonth,
                NewEndYear = source.NewEndYear,
                NewPrice = source.NewPrice
            };

            return Task.FromResult(result);
        }
    }
}
