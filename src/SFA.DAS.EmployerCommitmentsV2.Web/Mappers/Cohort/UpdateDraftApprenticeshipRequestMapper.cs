using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class UpdateDraftApprenticeshipRequestMapper : IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipRequest>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public UpdateDraftApprenticeshipRequestMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<UpdateDraftApprenticeshipRequest> Map(EditDraftApprenticeshipViewModel source)
        {
            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.CohortId.Value, source.DraftApprenticeshipId);

            return new UpdateDraftApprenticeshipRequest
            {
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                CourseOption = draftApprenticeship.TrainingCourseOption,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                Reference = source.Reference
            };
        }
    }
}
