using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipRequestToSelectCourseViewModelMapper : IMapper<AddDraftApprenticeshipRequest, SelectCourseViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;

        public AddDraftApprenticeshipRequestToSelectCourseViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
        }

        public async Task<SelectCourseViewModel> Map(AddDraftApprenticeshipRequest source)
        {
            var cohortId = _encodingService.Decode(source.CohortReference, EncodingType.CohortReference);
            var accountLegalEntityId = _encodingService.Decode(source.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId);
            var cohort = await _commitmentsApiClient.GetCohort(cohortId);

            if (cohort.WithParty != Party.Employer)
                throw new CohortEmployerUpdateDeniedException($"Cohort {cohort} is not with the Employer");

            var courses = cohort.IsFundedByTransfer || cohort.LevyStatus == ApprenticeshipEmployerType.NonLevy
                ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes
                : (await _commitmentsApiClient.GetAllTrainingProgrammes()).TrainingProgrammes;

            return new SelectCourseViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                AccountLegalEntityId = accountLegalEntityId,
                CohortId = cohortId,
                CohortReference = source.CohortReference,
                CourseCode = source.CourseCode,
                Courses = courses.ToArray(),
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
                DeliveryModel = source.DeliveryModel
            };
        }
    }
}
