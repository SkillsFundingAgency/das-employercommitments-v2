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
                CohortId = source.CohortId,
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference,
                DraftApprenticeshipId = draftApprenticeship.Id,
                FirstName = draftApprenticeship.FirstName,
                LastName = draftApprenticeship.LastName,
                Email = draftApprenticeship.Email,
                Uln = draftApprenticeship.Uln,
                StandardTitle = draftApprenticeship.TrainingCourseName,
                StandardUId = draftApprenticeship.StandardUId,
                CourseCode = draftApprenticeship.CourseCode,
                Version = draftApprenticeship.TrainingCourseVersion,
                CourseVersionConfirmed = draftApprenticeship.TrainingCourseVersionConfirmed,
                CourseOption = draftApprenticeship.TrainingCourseOption,
                Cost = draftApprenticeship.Cost,
                ReservationId = draftApprenticeship.ReservationId,
                Reference = draftApprenticeship.Reference, 
                Options = standard.TrainingProgramme.Options,
                StandardUrl = standard.TrainingProgramme.StandardPageUrl
            };
        }
    }
}