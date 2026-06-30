using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ChangePaymentsRequestViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public string ApprenticeshipHashedId { get; set; }
    public long ApprenticeshipId { get; set; }
    public long AccountId { get; set; }
    public string ApprenticeName { get; set; }
    public string ULN { get; set; }
    public string Course { get; set; }
    public bool FreezeStatus { get; set; }
    public bool? ChangeConfirmed { get; set; }
    public int? FreezePaymentsReason { get; set; }
    public DateTime PauseDate { get; set; }
    public DateTime? ResumeDate { get; set; }
}
