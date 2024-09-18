using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangeOptionViewModelMapper : IMapper<ChangeOptionRequest, ChangeOptionViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly ICacheStorageService _cacheStorageService;

    public ChangeOptionViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ICacheStorageService cacheStorageService)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _cacheStorageService = cacheStorageService;
    }

    public async Task<ChangeOptionViewModel> Map(ChangeOptionRequest source)
    {
        var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);

        var editViewModel = await _cacheStorageService.RetrieveFromCache<EditApprenticeshipRequestViewModel>(nameof(EditApprenticeshipRequestViewModel));

        string selectedVersion;
        string selectedCourseCode;
        string selectedOption;

        var returnToChangeVersion = false;
        var returnToEdit = false;

        if (editViewModel != null)
        {
            selectedCourseCode = editViewModel.CourseCode;
            selectedVersion = editViewModel.Version;
            selectedOption = editViewModel.Option;

            if (selectedCourseCode != apprenticeship.CourseCode || editViewModel.StartDate.Date.Value != apprenticeship.StartDate?.Date)
            {
                returnToEdit = true;
            }
            else if (selectedVersion != apprenticeship.Version)
            {
                returnToChangeVersion = true;
            }
        }
        else
        {
            selectedCourseCode = apprenticeship.CourseCode;
            selectedVersion = apprenticeship.Version;
            selectedOption = apprenticeship.Option;
        }

        var standardVersion = await _commitmentsApiClient.GetTrainingProgrammeVersionByCourseCodeAndVersion(selectedCourseCode, selectedVersion);

        return new ChangeOptionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            CurrentOption = apprenticeship.Option == string.Empty ? "TBC" : apprenticeship.Option,
            SelectedVersion = selectedVersion,
            SelectedOption = selectedOption,
            SelectedVersionName = standardVersion.TrainingProgramme.Name,
            SelectedVersionUrl = standardVersion.TrainingProgramme.StandardPageUrl,
            Options = standardVersion.TrainingProgramme.Options,
            ReturnToChangeVersion = returnToChangeVersion,
            ReturnToEdit = returnToEdit,
            CacheKey = source.CacheKey
        };
    }
}