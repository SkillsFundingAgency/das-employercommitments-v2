using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class SelectOptionViewModelMapper : IMapper<SelectOptionRequest, SelectOptionViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public SelectOptionViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<SelectOptionViewModel> Map(SelectOptionRequest source)
        {
            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.CohortId, source.DraftApprenticeshipId);

            if (draftApprenticeship.HasStandardOptions == false)
            {
                return null;
            }

            var standard = await _commitmentsApiClient.GetTrainingProgrammeVersionByStandardUId(draftApprenticeship.StandardUId);

            return new SelectOptionViewModel(draftApprenticeship.DateOfBirth, draftApprenticeship.StartDate, draftApprenticeship.EndDate)
            {
                DraftApprenticeshipId = draftApprenticeship.Id,
                FirstName = draftApprenticeship.FirstName,
                LastName = draftApprenticeship.LastName,
                Email = draftApprenticeship.Email,
                Uln = draftApprenticeship.Uln,
                CohortId = source.CohortId,
                CourseCode = draftApprenticeship.CourseCode,
                CourseOption = draftApprenticeship.TrainingCourseOption,
                Cost = draftApprenticeship.Cost,
                CohortReference = source.CohortReference,
                ReservationId = draftApprenticeship.ReservationId,
                Reference = draftApprenticeship.Reference, 
                AccountHashedId = source.AccountHashedId,
                Options = standard.TrainingProgramme.Options,
                StandardTitle = draftApprenticeship.TrainingCourseName,
                StandardUId = draftApprenticeship.StandardUId,
                StandardUrl = standard.TrainingProgramme.StandardPageUrl,
                Version = draftApprenticeship.TrainingCourseVersion,
                CourseVersionConfirmed = draftApprenticeship.TrainingCourseVersionConfirmed
            };
        }
    }
}