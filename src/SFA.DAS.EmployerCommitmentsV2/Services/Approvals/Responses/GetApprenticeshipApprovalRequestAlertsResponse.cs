namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetApprenticeshipApprovalRequestAlertsResponse
{
    public string ApprenticeName { get; set; }
    public List<ApprovalRequestItem> ApprovalRequests { get; set; }
}