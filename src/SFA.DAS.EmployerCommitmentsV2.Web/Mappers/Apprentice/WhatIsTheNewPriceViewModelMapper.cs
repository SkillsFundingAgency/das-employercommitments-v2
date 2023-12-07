using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class WhatIsTheNewPriceViewModelMapper : IMapper<ChangeOfProviderRequest, WhatIsTheNewPriceViewModel>
{
    private readonly ICommitmentsApiClient _client;

    public WhatIsTheNewPriceViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<WhatIsTheNewPriceViewModel> Map(ChangeOfProviderRequest source)
    {
        var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId.Value);

        return new WhatIsTheNewPriceViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ProviderName = source.ProviderName,
            ProviderId = source.ProviderId.Value,
            StopDate = apprenticeship.StopDate.Value,
            NewStartMonthYear = $"{source.NewStartMonth.Value}{source.NewStartYear.Value}",
            NewEndMonthYear = $"{source.NewEndMonth.Value}{source.NewEndYear.Value}",
            Edit = source.Edit ?? false,
            NewStartMonth = source.NewStartMonth,
            NewStartYear = source.NewStartYear,
            NewEndMonth = source.NewEndMonth,
            NewEndYear = source.NewEndYear,
            NewPrice = source.NewPrice,
            StoppedDuringCoP = source.StoppedDuringCoP
        };
    }
}