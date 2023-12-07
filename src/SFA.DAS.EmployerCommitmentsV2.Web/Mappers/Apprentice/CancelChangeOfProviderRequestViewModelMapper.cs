using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class CancelChangeOfProviderRequestViewModelMapper : IMapper<ChangeOfProviderRequest, CancelChangeOfProviderRequestViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public CancelChangeOfProviderRequestViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<CancelChangeOfProviderRequestViewModel> Map(ChangeOfProviderRequest source)
    {
        var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId.Value, CancellationToken.None);
            
        return new CancelChangeOfProviderRequestViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ProviderName = source.ProviderName,
            ProviderId = source.ProviderId.Value,
            EmployerWillAdd = source.EmployerWillAdd,
            NewStartMonth = source.NewStartMonth,
            NewStartYear = source.NewStartYear,
            NewEndMonth = source.NewEndMonth,
            NewEndYear = source.NewEndYear,
            NewPrice = source.NewPrice,
            ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
            OldProviderName = apprenticeship.ProviderName,
            StoppedDuringCoP = source.StoppedDuringCoP
        };
    }
}