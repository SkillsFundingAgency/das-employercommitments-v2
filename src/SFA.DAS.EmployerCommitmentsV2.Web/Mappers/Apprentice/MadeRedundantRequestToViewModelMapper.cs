using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class MadeRedundantRequestToViewModelMapper : IMapper<MadeRedundantRequest, MadeRedundantViewModel>
{
    private readonly ICommitmentsApiClient _client;
    public MadeRedundantRequestToViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<MadeRedundantViewModel> Map(MadeRedundantRequest source)
    {
        var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

        return new MadeRedundantViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            IsCoPJourney = source.IsCoPJourney,
            StopMonth = source.StopMonth,
            StopYear = source.StopYear,
            ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
        };
    }
}