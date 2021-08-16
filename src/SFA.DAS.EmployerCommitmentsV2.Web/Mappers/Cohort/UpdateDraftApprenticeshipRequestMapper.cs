using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class UpdateDraftApprenticeshipRequestMapper : IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipRequest>
    {
        private ICommitmentsApiClient _commitmentsAPiClient;

        public UpdateDraftApprenticeshipRequestMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsAPiClient = commitmentsApiClient;
        }

        public async Task<UpdateDraftApprenticeshipRequest> Map(EditDraftApprenticeshipViewModel source)
        {
            var standard = await _commitmentsAPiClient.GetCalculatedTrainingProgrammeVersion(int.Parse(source.CourseCode), source.StartDate.Date.Value);

            var selectedOption = standard.TrainingProgramme.StandardUId == source.StandardUId ? source.CourseOption : null;

            return new UpdateDraftApprenticeshipRequest
            {
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                StandardUId = standard.TrainingProgramme.StandardUId,
                CourseVersion = standard.TrainingProgramme.Version,
                CourseVersionConfirmed = true,
                CourseCode = source.CourseCode,
                CourseOption = selectedOption,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                Reference = source.Reference
            };
        }
    }
}
