using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddDraftApprenticeshipRequestMapper : IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipApimRequest>
{
    private readonly IAuthenticationService _authenticationService;

    public AddDraftApprenticeshipRequestMapper(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public Task<AddDraftApprenticeshipApimRequest> Map(AddDraftApprenticeshipViewModel source)
    {
        return Task.FromResult(new AddDraftApprenticeshipApimRequest
        {
            UserId = _authenticationService.UserId,
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
            EndDate = (source.IsOnFlexiPaymentPilot ?? false) ? source.ActualEndDate : source.EndDate.Date,
            EmploymentEndDate = source.EmploymentEndDate.Date,
            OriginatorReference = source.Reference,
            ReservationId = source.ReservationId,
            ActualStartDate = source.ActualStartDate,
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