using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class ViewDraftApprenticeshipViewModelMapper : IMapper<ViewDraftApprenticeshipRequest, IDraftApprenticeshipViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IApprovalsApiClient _outerApiClient;

    public ViewDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient outerApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _outerApiClient = outerApiClient;
    }

    public async Task<IDraftApprenticeshipViewModel> Map(ViewDraftApprenticeshipRequest source)
    {
        var draftApprenticeship = await _outerApiClient.GetViewDraftApprenticeship(source.Cohort.AccountId,
            source.Cohort.CohortId, source.Request.DraftApprenticeshipId);

        var trainingCourse = string.IsNullOrWhiteSpace(draftApprenticeship.CourseCode) ? null
            : await _commitmentsApiClient.GetTrainingProgramme(draftApprenticeship.CourseCode);
            
        var result = new ViewDraftApprenticeshipViewModel
        {
            AccountHashedId = source.Request.AccountHashedId,
            CohortReference = source.Request.CohortReference,
            FirstName = draftApprenticeship.FirstName,
            LastName = draftApprenticeship.LastName,
            Email = draftApprenticeship.Email,
            Uln = draftApprenticeship.Uln,
            DateOfBirth = draftApprenticeship.DateOfBirth,
            DeliveryModel = draftApprenticeship.DeliveryModel,
            TrainingCourse = trainingCourse?.TrainingProgramme.Name,
            HasStandardOptions = draftApprenticeship.HasStandardOptions,
            Version = draftApprenticeship.TrainingCourseVersion,
            CourseOption = GetCourseOption(draftApprenticeship.TrainingCourseOption),
            Cost = draftApprenticeship.Cost,
            EmploymentPrice = draftApprenticeship.EmploymentPrice,
            StartDate = draftApprenticeship.StartDate,
            EndDate = draftApprenticeship.EndDate,
            EmploymentEndDate = draftApprenticeship.EmploymentEndDate,
            Reference = draftApprenticeship.Reference,
            LegalEntityName = source.Cohort.LegalEntityName,
            RecognisePriorLearning = draftApprenticeship.RecognisePriorLearning,
            TrainingTotalHours = draftApprenticeship.TrainingTotalHours,
            DurationReducedByHours = draftApprenticeship.DurationReducedByHours,
            DurationReducedBy = draftApprenticeship.DurationReducedBy,
            PriceReducedBy = draftApprenticeship.PriceReducedBy,
            ActualStartDate = draftApprenticeship.ActualStartDate,
            IsOnFlexiPaymentPilot = draftApprenticeship.IsOnFlexiPaymentPilot
        };

        return result;
    }

    private string GetCourseOption(string draftApprenticeshipTrainingCourseOption)
    {
        return draftApprenticeshipTrainingCourseOption switch
        {
            null => string.Empty,
            "" => "To be confirmed",
            _ => draftApprenticeshipTrainingCourseOption
        };
    }
}