using System.Threading.Tasks;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using System;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class ViewDraftApprenticeshipViewModelMapper : IMapper<ViewDraftApprenticeshipRequest, IDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IApprovalsApiClient _outerApiClient;
        private readonly IEncodingService _encodingService;

        public ViewDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient outerApiClient, IEncodingService encodingService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _outerApiClient = outerApiClient;
            _encodingService = encodingService;
        }

        public async Task<IDraftApprenticeshipViewModel> Map(ViewDraftApprenticeshipRequest source)
        {
            var draftApprenticeshipId = _encodingService.Decode(source.Request.DraftApprenticeshipHashedId, EncodingType.ApprenticeshipId);
            var draftApprenticeship = await _outerApiClient.GetViewDraftApprenticeship(source.Cohort.AccountId,
                source.Cohort.CohortId, draftApprenticeshipId);

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
}
