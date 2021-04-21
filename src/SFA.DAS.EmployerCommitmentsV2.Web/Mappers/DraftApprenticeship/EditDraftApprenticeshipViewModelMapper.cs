using System.Threading.Tasks;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{

    public class EditDraftApprenticeshipViewModelMapper : IMapper<EditDraftApprenticeshipRequest, IDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly IAuthorizationService _authorizationService;

        public EditDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient,
            IEncodingService encodingService, IAuthorizationService authorizationService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _authorizationService = authorizationService;
        }

        public async Task<IDraftApprenticeshipViewModel> Map(EditDraftApprenticeshipRequest source)
        {
            var cohort = source.Cohort;

            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.Request.CohortId, source.Request.DraftApprenticeshipId);

            return new EditDraftApprenticeshipViewModel(draftApprenticeship.DateOfBirth, draftApprenticeship.StartDate, draftApprenticeship.EndDate)
            {
                DraftApprenticeshipId = draftApprenticeship.Id,
                DraftApprenticeshipHashedId = _encodingService.Encode(draftApprenticeship.Id, EncodingType.ApprenticeshipId),
                CohortId = source.Request.CohortId,
                CohortReference = _encodingService.Encode(source.Request.CohortId, EncodingType.CohortReference),
                ReservationId = draftApprenticeship.ReservationId,
                FirstName = draftApprenticeship.FirstName,
                LastName = draftApprenticeship.LastName,
                Email = draftApprenticeship.Email,
                Uln = draftApprenticeship.Uln,
                CourseCode = draftApprenticeship.CourseCode,
                Cost = draftApprenticeship.Cost,
                Reference = draftApprenticeship.Reference,
                AccountHashedId = source.Request.AccountHashedId,
                ProviderName = cohort.ProviderName,
                LegalEntityName = source.Cohort.LegalEntityName,
                IsContinuation = draftApprenticeship.IsContinuation,
                Courses = (cohort.IsFundedByTransfer || cohort.LevyStatus == ApprenticeshipEmployerType.NonLevy) && !draftApprenticeship.IsContinuation
                    ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes
                    : (await _commitmentsApiClient.GetAllTrainingProgrammes()).TrainingProgrammes,
                ShowEmail = await _authorizationService.IsAuthorizedAsync(EmployerFeature.ApprenticeEmail)
            };
        }
    }
}
