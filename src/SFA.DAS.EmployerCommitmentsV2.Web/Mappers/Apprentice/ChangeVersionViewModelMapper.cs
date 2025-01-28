using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangeVersionViewModelMapper(ICommitmentsApiClient commitmentsApiClient) : IMapper<ChangeVersionRequest, ChangeVersionViewModel>
{
    public async Task<ChangeVersionViewModel> Map(ChangeVersionRequest source)
    {
        var apprenticeship = await commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);

        var currentVersion = await commitmentsApiClient.GetTrainingProgrammeVersionByStandardUId(apprenticeship.StandardUId);

        var newerVersions = await commitmentsApiClient.GetNewerTrainingProgrammeVersions(apprenticeship.StandardUId);

        return new ChangeVersionViewModel
        {
            CurrentVersion = apprenticeship.Version,
            StandardTitle = currentVersion.TrainingProgramme.Name,
            StandardUrl = currentVersion.TrainingProgramme.StandardPageUrl,
            NewerVersions = newerVersions.NewerVersions.Select(x => x.Version),
            CacheKey = source.CacheKey,
            
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
        };
    }
}