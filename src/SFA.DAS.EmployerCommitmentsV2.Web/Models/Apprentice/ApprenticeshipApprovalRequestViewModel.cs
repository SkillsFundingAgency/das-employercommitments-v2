
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ApprenticeshipApprovalRequestViewModel : IAuthorizationContextModel
{
    public string ApprenticeshipHashedId { get; set; }
    public long ApprenticeshipId { get; set; }
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }
    public Guid ApprovalRequestId { get; set; }
    public CocApprovalResultStatus? ApprovalRequestStatus { get; set; }
    public virtual ICollection<ChangeItem> Items { get; set; }
    public string Name { get; set; }
    public string ULN { get; set; }
    public string CourseName { get; set; }
    public string ProviderName { get; set; }
    public long UKPRN { get; set; }
    public bool? ApproveChanges { get; set; }
    public bool ExceedsFundingCap { get; set; }
    public string DisplayFundingCapPrice { get; set; }
    public bool IsSupersededOrCancelled { get; set; }

    public class ChangeItem
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
    }
}