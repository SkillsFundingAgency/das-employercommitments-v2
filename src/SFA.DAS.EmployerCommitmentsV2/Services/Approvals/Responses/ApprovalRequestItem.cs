namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class ApprovalRequestItem
{
    public Guid Id { get; set; }
    public Guid LearningKey { get; set; }
    public long ApprenticeshipId { get; set; }
    public byte LearningType { get; set; }
    public byte? Status { get; set; }
    public List<ApprovalFieldRequest> Items { get; set; }
}

public class ApprovalFieldRequest
{
    public string Field { get; set; }
    public string Old { get; set; }
    public string New { get; set; }
    public byte? Status { get; set; }
    public DateTime Created { get; set; }
}