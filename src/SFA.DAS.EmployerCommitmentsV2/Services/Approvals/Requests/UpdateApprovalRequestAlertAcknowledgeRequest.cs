using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

public class UpdateApprovalRequestAlertAcknowledgeRequest : ApimSaveDataRequest
{
    public List<UpdateApprovalRequestAlertAcknowledge> ApprovalRequestAlerts { get; set; }
}

public class UpdateApprovalRequestAlertAcknowledge
{
    public Guid ApprovalRequestId { get; set; }
    public bool Acknowledged { get; set; }
    public ApimUserInfo UserInfo { get; set; }
}