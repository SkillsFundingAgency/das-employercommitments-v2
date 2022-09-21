using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using AddDraftApprenticeshipRequest = SFA.DAS.CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class AddDraftApprenticeshipRequestMapper : IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipRequest>
    {
       public Task<AddDraftApprenticeshipRequest> Map(AddDraftApprenticeshipViewModel source)
        {
            return Task.FromResult(new AddDraftApprenticeshipRequest
            {
                UserId = "X", // TODO: Remove this from the request as it's not required
                ProviderId = 1, // TODO: Remove this from the request as it's not required
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth.Date,
                Email = source.Email, 
                Uln = source.Uln,
                DeliveryModel = source.DeliveryModel,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                EmploymentPrice = source.EmploymentPrice,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                EmploymentEndDate = source.EmploymentEndDate.Date,
                OriginatorReference = source.Reference,
                ReservationId = source.ReservationId,
                ActualStartDate = source.ActualStartDate,
                IsOnFlexiPaymentPilot = source.IsOnFlexiPaymentPilot
            });
        }
    }
}