using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class ViewDraftApprenticeshipViewModelMapper : IMapper<ViewDraftApprenticeshipRequest, IDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public ViewDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ITrainingProgrammeApiClient trainingProgrammeApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
        }

        public async Task<IDraftApprenticeshipViewModel> Map(ViewDraftApprenticeshipRequest source)
        {
            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.Request.CohortId, source.Request.DraftApprenticeshipId);

            var trainingCourse = string.IsNullOrWhiteSpace(draftApprenticeship.CourseCode) ? null
                :await _trainingProgrammeApiClient.GetTrainingProgramme(draftApprenticeship.CourseCode);
            
            var result = new ViewDraftApprenticeshipViewModel
            {
                AccountHashedId = source.Request.AccountHashedId,
                CohortReference = source.Request.CohortReference,
                FirstName = draftApprenticeship.FirstName,
                LastName = draftApprenticeship.LastName,
                Uln = draftApprenticeship.Uln,
                DateOfBirth = draftApprenticeship.DateOfBirth,
                TrainingCourse = trainingCourse == null ? "" : trainingCourse.ExtendedTitle,
                Cost = draftApprenticeship.Cost,
                StartDate = draftApprenticeship.StartDate,
                EndDate = draftApprenticeship.EndDate,
                Reference = draftApprenticeship.Reference
            };

            return await Task.FromResult(result);
        }
    }
}
