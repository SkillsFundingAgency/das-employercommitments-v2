using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipViewModelMapper : IMapper<AddDraftApprenticeshipRequest, AddDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public AddDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ITrainingProgrammeApiClient trainingProgrammeApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
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
                ReservationId = source.ReservationId,
                StartDate = new MonthYearModel(source.StartMonthYear),
                CourseCode = source.CourseCode,
                ProviderName = cohort.ProviderName,
                Courses = cohort.IsFundedByTransfer
                    ? await _trainingProgrammeApiClient.GetStandardTrainingProgrammes()
                    : await _trainingProgrammeApiClient.GetAllTrainingProgrammes()
            };

            return result;
        }
    }
}
