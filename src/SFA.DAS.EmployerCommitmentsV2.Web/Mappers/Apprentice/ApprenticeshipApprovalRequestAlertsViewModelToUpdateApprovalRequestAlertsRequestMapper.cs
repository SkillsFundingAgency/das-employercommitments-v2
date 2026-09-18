using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ApprenticeshipApprovalRequestAlertsViewModelToUpdateApprovalRequestAlertsRequestMapper(IAuthenticationService authenticationService) : IMapper<ApprenticeshipApprovalRequestAlertsViewModel, UpdateApprovalRequestAlertAcknowledgeRequest>
{
    public Task<UpdateApprovalRequestAlertAcknowledgeRequest> Map(ApprenticeshipApprovalRequestAlertsViewModel source)
    {
        return Task.FromResult(new UpdateApprovalRequestAlertAcknowledgeRequest
        {
            ApprovalRequestAlerts = [.. source.ApprovalRequests.Select(r => new UpdateApprovalRequestAlertAcknowledge
            {
                ApprovalRequestId = r.Id,
                Acknowledged = r.Seen.HasValue && r.Seen.Value,
                UserInfo= new ApimUserInfo
                {
                    UserId = authenticationService.UserId,
                    UserDisplayName = authenticationService.UserName,
                    UserEmail = authenticationService.UserEmail
                }
            })]
        });
    }
}