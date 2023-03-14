using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class UpdateDraftApprenticeshipRequestMapper : IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipApimRequest>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public UpdateDraftApprenticeshipRequestMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<UpdateDraftApprenticeshipApimRequest> Map(EditDraftApprenticeshipViewModel source)
        {
            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.CohortId.Value, source.DraftApprenticeshipId);

            return new UpdateDraftApprenticeshipApimRequest
            {
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                DeliveryModel = source.DeliveryModel.Value,
                CourseCode = source.CourseCode,
                CourseOption = draftApprenticeship.TrainingCourseOption,
                Cost = source.Cost,
                EmploymentPrice = source.EmploymentPrice,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                EmploymentEndDate = source.EmploymentEndDate.Date,
                Reference = source.Reference,
                IsOnFlexiPaymentPilot = source.IsOnFlexiPaymentPilot,
                ActualStartDate = source.ActualStartDate
            };
        }
    }
}
