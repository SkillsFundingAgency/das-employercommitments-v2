using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class StopRequestToViewModelMapper : IMapper<StopRequest, StopRequestViewModel>
{
    private readonly ICommitmentsApiClient _client;
    public StopRequestToViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<StopRequestViewModel> Map(StopRequest source)
    {
        var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

        return new StopRequestViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            StartDate = apprenticeship.StartDate.Value,
            IsCoPJourney = source.IsCoPJourney
        };
    }
}