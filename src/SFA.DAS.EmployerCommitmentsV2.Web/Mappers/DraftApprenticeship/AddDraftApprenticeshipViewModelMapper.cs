using System.Threading.Tasks;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipViewModelMapper : IMapper<AddDraftApprenticeshipRequest, AddDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly IAuthorizationService _authorizationService;

        public AddDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient,
            IEncodingService encodingService, IAuthorizationService authorizationService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _authorizationService = authorizationService;
        }

        public async Task<AddDraftApprenticeshipViewModel> Map(AddDraftApprenticeshipRequest source)
        {
            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

            if (cohort.WithParty != Party.Employer)
                throw new CohortEmployerUpdateDeniedException($"Cohort {cohort} is not with the Employer");

            var result = new AddDraftApprenticeshipViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference,
                CohortId = source.CohortId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                LegalEntityName = cohort.LegalEntityName,
                ReservationId = source.ReservationId,
                StartDate = new MonthYearModel(source.StartMonthYear),
                CourseCode = source.CourseCode,
                ProviderName = cohort.ProviderName,
                Courses = cohort.IsFundedByTransfer || cohort.LevyStatus == ApprenticeshipEmployerType.NonLevy
                    ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes
                    : (await _commitmentsApiClient.GetAllTrainingProgrammes()).TrainingProgrammes,    
                TransferSenderHashedId = cohort.IsFundedByTransfer ? _encodingService.Encode(cohort.TransferSenderId.Value, EncodingType.PublicAccountId) : string.Empty,
                AutoCreatedReservation = source.AutoCreated,
                ShowEmail = await _authorizationService.IsAuthorizedAsync(EmployerFeature.ApprenticeEmail)
            };

            return result;
        }
    }
}
