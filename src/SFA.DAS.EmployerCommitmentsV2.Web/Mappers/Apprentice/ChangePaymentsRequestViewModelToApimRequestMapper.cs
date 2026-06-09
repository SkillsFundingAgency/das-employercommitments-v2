using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangePaymentsRequestViewModelToApimRequestMapper(IAuthenticationService authenticationService)
    : IMapper<ChangePaymentsRequestViewModel, ChangePaymentsApimRequest>
{
    public Task<ChangePaymentsApimRequest> Map(ChangePaymentsRequestViewModel source)
    {
        var isPausing = !source.FreezeStatus;

        return Task.FromResult(new ChangePaymentsApimRequest
        {
            PaymentFreezeDate = isPausing ? source.PauseDate : null,
            FreezePaymentsReason = isPausing ? source.FreezePaymentsReason : null,
            UserInfo = new ApimUserInfo
            {
                UserDisplayName = authenticationService.UserName,
                UserEmail = authenticationService.UserEmail,
                UserId = authenticationService.UserId
            }
        });
    }
}
