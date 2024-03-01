using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangeVersionViewModelMapper : IMapper<ChangeVersionRequest, ChangeVersionViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
        
    public ChangeVersionViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<ChangeVersionViewModel> Map(ChangeVersionRequest source)
    {
        var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);

        var currentVersion = await _commitmentsApiClient.GetTrainingProgrammeVersionByStandardUId(apprenticeship.StandardUId);

        var newerVersions = await _commitmentsApiClient.GetNewerTrainingProgrammeVersions(apprenticeship.StandardUId);

        return new ChangeVersionViewModel
        {
            CurrentVersion = apprenticeship.Version,
            StandardTitle = currentVersion.TrainingProgramme.Name,
            StandardUrl = currentVersion.TrainingProgramme.StandardPageUrl,
            NewerVersions = newerVersions.NewerVersions.Select(x => x.Version)
        };
    }
}