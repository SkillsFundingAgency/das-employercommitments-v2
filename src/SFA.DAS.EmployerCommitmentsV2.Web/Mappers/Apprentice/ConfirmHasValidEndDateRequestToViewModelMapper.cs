using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ConfirmHasValidEndDateRequestToViewModelMapper : IMapper<ConfirmHasValidEndDateRequest, ConfirmHasValidEndDateViewModel>
{
    private readonly ICommitmentsApiClient _client;

    public ConfirmHasValidEndDateRequestToViewModelMapper(ICommitmentsApiClient client)
    {
        _client = client;
    }

    public async Task<ConfirmHasValidEndDateViewModel> Map(ConfirmHasValidEndDateRequest source)
    {
        var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

        return new ConfirmHasValidEndDateViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
            ULN = apprenticeship.Uln,
            Course = apprenticeship.CourseName,
            EndDate = apprenticeship.EndDate
        };
    }
}