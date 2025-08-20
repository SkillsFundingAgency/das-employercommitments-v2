using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EditApprenticeshipRequestViewModelToSelectCourseViewModelMapper : IMapper<EditApprenticeshipRequestViewModel, SelectCourseViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public EditApprenticeshipRequestViewModelToSelectCourseViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        => _commitmentsApiClient = commitmentsApiClient;

    public async Task<SelectCourseViewModel> Map(EditApprenticeshipRequestViewModel source)
    {
        var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);
        var cohort = await _commitmentsApiClient.GetCohort(apprenticeship.CohortId);

        return new SelectCourseViewModel
        {
            CourseCode = source.CourseCode,
            CacheKey = source.CacheKey
        };
    }
}