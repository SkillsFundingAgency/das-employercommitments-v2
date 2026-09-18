using SFA.DAS.EmployerCommitmentsV2.Enums;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ApprenticeshipApprovalRequestAlertsViewModel
{
    public string ApprenticeName { get; set; }
    public string ApprenticeshipHashedId { get; set; }
    public long ApprenticeshipId { get; set; }
    public long AccountId { get; set; }
    public string AccountHashedId { get; set; }
    public List<ApprovalRequestAlertViewModel> ApprovalRequests { get; set; }
}

public class ApprovalRequestAlertViewModel
{
    public Guid Id { get; set; }
    public long ApprenticeshipId { get; set; }
    public byte? Status { get; set; }
    public DateTime? EmployerAcknowledgedAt { get; set; }
    public bool? Seen { get; set; }
    public List<ApprovalFieldRequestAlertViewModel> ApprovalRequestFieldItems { get; set; }
}

public class ApprovalFieldRequestAlertViewModel
{
    public string Field { get; set; }
    public string Old { get; set; }
    public string New { get; set; }
    public CocApprovalItemStatus? Status { get; set; }
    public DateTime Created { get; set; }
}