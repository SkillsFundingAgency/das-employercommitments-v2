using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class InformRequestToInformViewModelMapper : IMapper<InformRequest, InformViewModel>
{
    private readonly ICommitmentsApiClient _client;

    public InformRequestToInformViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<InformViewModel> Map(InformRequest source)
    {
        var account = await _client.GetAccount(source.AccountId);

        return new InformViewModel
        {
            AccountHashedId = source.AccountHashedId,
            LevyFunded = account.LevyStatus == ApprenticeshipEmployerType.Levy
        };
    }
}