using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ConfirmHasNotStopRequestToViewModelMapper : IMapper<ConfirmHasNotStopRequest, ConfirmHasNotStopViewModel>
{
    private readonly ICommitmentsApiClient _client;

    public ConfirmHasNotStopRequestToViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<ConfirmHasNotStopViewModel> Map(ConfirmHasNotStopRequest source)
    {
        var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

        return new ConfirmHasNotStopViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
            ULN = apprenticeship.Uln,
            Course = apprenticeship.CourseName
        };
    }
}