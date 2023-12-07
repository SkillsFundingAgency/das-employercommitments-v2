using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class WhatIsTheNewEndDateToChangeOfProviderRequestMapper : IMapper<WhatIsTheNewEndDateViewModel, ChangeOfProviderRequest>
{
    public Task<ChangeOfProviderRequest> Map(WhatIsTheNewEndDateViewModel source)
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
            NewPrice = source.NewPrice,
            StoppedDuringCoP = source.StoppedDuringCoP
        };

        return Task.FromResult(result);
    }
}