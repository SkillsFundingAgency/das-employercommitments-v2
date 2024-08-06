using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class EditDraftApprenticeshipRequestToSelectCourseViewModelMapper : IMapper<EditDraftApprenticeshipRequest, SelectCourseViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public EditDraftApprenticeshipRequestToSelectCourseViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        => _commitmentsApiClient = commitmentsApiClient;

    public async Task<SelectCourseViewModel> Map(EditDraftApprenticeshipRequest source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

        if (cohort.WithParty != Party.Employer)
            throw new CohortEmployerUpdateDeniedException($"Cohort {cohort} is not with the Employer");

        var courses = cohort.IsFundedByTransfer || cohort.LevyStatus == ApprenticeshipEmployerType.NonLevy
            ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes
            : (await _commitmentsApiClient.GetAllTrainingProgrammes()).TrainingProgrammes;

        return new SelectCourseViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            //AccountLegalEntityId = source.AccountLegalEntityId,
            //CohortId = source.CohortId,
            CohortReference = source.CohortReference,
            CourseCode = source.CourseCode,
            Courses = courses.ToArray(),
            //ProviderId = source.ProviderId,
            //ReservationId = source.ReservationId,
            //StartMonthYear = source.StartMonthYear,
            DeliveryModel = source.DeliveryModel
        };
    }
}