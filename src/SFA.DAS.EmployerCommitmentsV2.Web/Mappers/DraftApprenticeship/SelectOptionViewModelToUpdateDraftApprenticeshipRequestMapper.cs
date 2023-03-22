using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class SelectOptionViewModelToUpdateDraftApprenticeshipRequestMapper : IMapper<SelectOptionViewModel, UpdateDraftApprenticeshipApimRequest>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public SelectOptionViewModelToUpdateDraftApprenticeshipRequestMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<UpdateDraftApprenticeshipApimRequest> Map(SelectOptionViewModel source)
        {
            var response = await _commitmentsApiClient.GetDraftApprenticeship(source.CohortId.Value, source.DraftApprenticeshipId);

            return new UpdateDraftApprenticeshipApimRequest
            {
                FirstName = response.FirstName,
                LastName = response.LastName,
                Email = response.Email,
                DateOfBirth = response.DateOfBirth,
                Uln = response.Uln,
                CourseCode = response.CourseCode,
                CourseOption = source.CourseOption == "N/A" ? string.Empty : source.CourseOption,
                Cost = response.Cost,
                EmploymentPrice = response.EmploymentPrice,
                StartDate = response.StartDate,
                ActualStartDate = response.ActualStartDate,
                EndDate = response.EndDate,
                EmploymentEndDate = response.EmploymentEndDate,
                Reference = response.Reference,
                ReservationId = response.ReservationId,
                DeliveryModel = response.DeliveryModel,
                IsOnFlexiPaymentPilot = response.IsOnFlexiPaymentPilot
            };
        }
    }
}
