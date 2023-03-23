using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class CreateCohortRequestMapper : 
        IMapper<ApprenticeViewModel, CreateCohortApimRequest>
    {
        private readonly IAuthenticationService _authenticationService;

        public CreateCohortRequestMapper(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public Task<CreateCohortApimRequest> Map(ApprenticeViewModel source)
        {
            return Task.FromResult(new CreateCohortApimRequest
            {
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                DeliveryModel = source.DeliveryModel,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                EmploymentPrice = source.EmploymentPrice,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                EmploymentEndDate = source.EmploymentEndDate.Date,
                OriginatorReference = source.Reference,
                TransferSenderId = source.DecodedTransferSenderId,
                PledgeApplicationId = (int?)source.PledgeApplicationId,
                IsOnFlexiPaymentPilot = source.IsOnFlexiPaymentPilot,
                UserInfo = new ApimUserInfo
                {
                    UserDisplayName = _authenticationService.UserName,
                    UserEmail = _authenticationService.UserEmail,
                    UserId = _authenticationService.UserId
                }
            });
        }
     }
}
