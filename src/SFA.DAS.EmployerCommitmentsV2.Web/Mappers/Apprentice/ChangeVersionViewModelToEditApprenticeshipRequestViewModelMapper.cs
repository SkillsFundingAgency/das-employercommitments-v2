using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangeVersionViewModelToEditApprenticeshipRequestViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    : IMapper<ChangeVersionViewModel, EditApprenticeshipRequestViewModel>
{
    public async Task<EditApprenticeshipRequestViewModel> Map(ChangeVersionViewModel source)
    {
        var apprenticeshipTask = commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);
        var priceEpisodesTask = commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId);

        await Task.WhenAll(apprenticeshipTask, priceEpisodesTask);

        var apprenticeship = apprenticeshipTask.Result;
        var priceEpisodes = priceEpisodesTask.Result;

        var currentPrice = priceEpisodes.PriceEpisodes.GetPrice();

        var versionResponse = await commitmentsApiClient.GetTrainingProgrammeVersionByCourseCodeAndVersion(apprenticeship.CourseCode, source.SelectedVersion);

        var newStandardVersion = versionResponse.TrainingProgramme;

        var editRequestViewModel = new EditApprenticeshipRequestViewModel(apprenticeship.DateOfBirth, apprenticeship.StartDate, apprenticeship.EndDate, apprenticeship.EmploymentEndDate)
        {
            AccountHashedId = source.AccountHashedId,
            HashedApprenticeshipId = source.ApprenticeshipHashedId,
            ULN = apprenticeship.Uln,
            FirstName = apprenticeship.FirstName,
            LastName = apprenticeship.LastName,
            Email = apprenticeship.Email,
            Cost = currentPrice,
            CourseCode = apprenticeship.CourseCode,
            Version = source.SelectedVersion,
            TrainingName = apprenticeship.CourseName != newStandardVersion.Name ? newStandardVersion.Name : null,
            EmployerReference = apprenticeship.EmployerReference,
            HasOptions = newStandardVersion.Options.Any(),
            DeliveryModel = apprenticeship.DeliveryModel,
            CacheKey = source.CacheKey
        };

        return editRequestViewModel;
    }
}