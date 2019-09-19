using System;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class EditDraftApprenticeshipViewModelMapper : IMapper<EditDraftApprenticeshipRequest, EditDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public EditDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient,
            IEncodingService encodingService,
            ITrainingProgrammeApiClient trainingProgrammeApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
        }

        public async Task<EditDraftApprenticeshipViewModel> Map(EditDraftApprenticeshipRequest source)
        {
            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

            if (cohort.WithParty != Party.Employer)
            {
                throw new CohortEmployerUpdateDeniedException($"Cohort {cohort.CohortId} is not With the Employer");
            }

            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.CohortId, source.DraftApprenticeshipId);
            
            return new EditDraftApprenticeshipViewModel(draftApprenticeship.DateOfBirth, draftApprenticeship.StartDate, draftApprenticeship.EndDate)
            {
                DraftApprenticeshipId = draftApprenticeship.Id,
                DraftApprenticeshipHashedId = _encodingService.Encode(draftApprenticeship.Id, EncodingType.ApprenticeshipId),
                CohortId = source.CohortId,
                CohortReference = _encodingService.Encode(source.CohortId, EncodingType.CohortReference),
                ReservationId = draftApprenticeship.ReservationId,
                FirstName = draftApprenticeship.FirstName,
                LastName = draftApprenticeship.LastName,
                Uln = draftApprenticeship.Uln,
                CourseCode = draftApprenticeship.CourseCode,
                Cost = draftApprenticeship.Cost,
                Reference = draftApprenticeship.Reference,
                AccountHashedId = source.AccountHashedId,
                ProviderName = cohort.ProviderName,
                Courses = cohort.IsFundedByTransfer
                    ? await _trainingProgrammeApiClient.GetStandardTrainingProgrammes()
                    : await _trainingProgrammeApiClient.GetAllTrainingProgrammes()
            };
        }
    }
}
