using SFA.DAS.EmployerCommitmentsV2.Enums;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

public class GetApprovalRequestAlertRequest(long accountId, long apprenticeshipId)
{
    public long ApprenticeshipId { get; set; } = apprenticeshipId;
    public long AccountId { get; set; } = accountId;
    public byte Status { get; set; } = (byte)CocApprovalItemStatus.AutoApproved;
    public string GetUrl => $"employer/{AccountId}/apprentices/{ApprenticeshipId}/approval-requests?status={Status}";
}