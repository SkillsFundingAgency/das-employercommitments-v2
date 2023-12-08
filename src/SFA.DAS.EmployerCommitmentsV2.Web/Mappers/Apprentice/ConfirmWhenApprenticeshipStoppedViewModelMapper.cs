using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ConfirmWhenApprenticeshipStoppedViewModelMapper : IMapper<ConfirmWhenApprenticeshipStoppedRequest, ConfirmWhenApprenticeshipStoppedViewModel>
{
    private readonly ICommitmentsApiClient _client;

    public ConfirmWhenApprenticeshipStoppedViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<ConfirmWhenApprenticeshipStoppedViewModel> Map(ConfirmWhenApprenticeshipStoppedRequest source)
    {
        var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

        return new ConfirmWhenApprenticeshipStoppedViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
            ULN = apprenticeship.Uln,
            Course = apprenticeship.CourseName,
            StopDate = apprenticeship.StopDate.GetValueOrDefault(),
        };
    }
}