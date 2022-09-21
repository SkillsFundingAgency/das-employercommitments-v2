using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class DraftApprenticeshipViewModelToValidateApimRequestMapper : IMapper<DraftApprenticeshipViewModel, ValidateDraftApprenticeshipApimRequest>
    {
        public Task<ValidateDraftApprenticeshipApimRequest> Map(DraftApprenticeshipViewModel source)
        {
            var destination = new ValidateDraftApprenticeshipApimRequest
            {
                CohortId = source.CohortId,
                ProviderId = source.ProviderId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
                EmploymentPrice = source.EmploymentPrice,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EmploymentEndDate = source.EmploymentEndDate.Date,
                EndDate = source.EndDate.Date,
                OriginatorReference = source.Reference,
                ReservationId = source.ReservationId,
                IgnoreStartDateOverlap = true,
                IsOnFlexiPaymentPilot = source.IsOnFlexiPaymentPilot
            };

            return Task.FromResult(destination);
        }
    }
}
