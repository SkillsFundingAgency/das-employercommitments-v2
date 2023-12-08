using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ReconfirmHasNotStopViewModelMapper : IMapper<ReConfirmHasNotStopRequest, ReconfirmHasNotStopViewModel>
{
    private readonly ICommitmentsApiClient _client;

    public ReconfirmHasNotStopViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<ReconfirmHasNotStopViewModel> Map(ReConfirmHasNotStopRequest source)
    {
        var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

        return new ReconfirmHasNotStopViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
            ULN = apprenticeship.Uln,
            Course = apprenticeship.CourseName
        };
    }
}