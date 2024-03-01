using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class SelectOptionViewModelMapper : IMapper<SelectOptionRequest, SelectOptionViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public SelectOptionViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<SelectOptionViewModel> Map(SelectOptionRequest source)
    {
        var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.CohortId, source.DraftApprenticeshipId);

        if (draftApprenticeship.HasStandardOptions == false)
        {
            return null;
        }

        var standard = await _commitmentsApiClient.GetTrainingProgrammeVersionByStandardUId(draftApprenticeship.StandardUId);

        return new SelectOptionViewModel(draftApprenticeship.DateOfBirth, draftApprenticeship.StartDate, draftApprenticeship.EndDate)
        {
            CohortId = source.CohortId,
            CohortReference = source.CohortReference,
            DraftApprenticeshipId = draftApprenticeship.Id,
            Version = draftApprenticeship.TrainingCourseVersion,
            StandardTitle = draftApprenticeship.TrainingCourseName,
            CourseOption = draftApprenticeship.TrainingCourseOption,
            Options = standard.TrainingProgramme.Options,
            StandardUrl = standard.TrainingProgramme.StandardPageUrl,
            DeliveryModel = draftApprenticeship.DeliveryModel,
            Cost = draftApprenticeship.Cost,
            EmploymentPrice = draftApprenticeship.EmploymentPrice,
            EmploymentEndMonth = draftApprenticeship.EmploymentEndDate.HasValue ? draftApprenticeship.EmploymentEndDate.Value.Month : (int?)null,
            EmploymentEndYear = draftApprenticeship.EmploymentEndDate.HasValue ? draftApprenticeship.EmploymentEndDate.Value.Year : (int?)null,
        };
    }
}