using System.Threading.Tasks;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class ViewDraftApprenticeshipViewModelMapper : IMapper<ViewDraftApprenticeshipRequest, IDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IAuthorizationService _authorizationService;

        public ViewDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IAuthorizationService authorizationService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _authorizationService = authorizationService;
        }

        public async Task<IDraftApprenticeshipViewModel> Map(ViewDraftApprenticeshipRequest source)
        {
            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.Request.CohortId, source.Request.DraftApprenticeshipId);

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
                TrainingCourse = trainingCourse?.TrainingProgramme.Name,
                Cost = draftApprenticeship.Cost,
                StartDate = draftApprenticeship.StartDate,
                EndDate = draftApprenticeship.EndDate,
                Reference = draftApprenticeship.Reference,
                LegalEntityName = source.Cohort.LegalEntityName,
                ShowEmail = await _authorizationService.IsAuthorizedAsync(EmployerFeature.ApprenticeEmail)
            };

            return result;
        }
    }
}
