namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

public class GetApprovalRequestAlertRequest(long accountId, long apprenticeshipId)
{
    public long ApprenticeshipId { get; set; } = apprenticeshipId;
    public long AccountId { get; set; } = accountId;
    public byte Status { get; set; } = 1;
    public string GetUrl => $"employer/{AccountId}/apprentices/{ApprenticeshipId}/approval-requests?status={Status}";
}