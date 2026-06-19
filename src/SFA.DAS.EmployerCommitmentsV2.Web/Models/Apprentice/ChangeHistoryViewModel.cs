using System.ComponentModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ChangeHistoryListViewModel
{
    public List<ChangeHistoryViewModel> ChangeHistory { get; set; } = [];

    public string Name { get; set; }

    public string ApprenticeshipHashedId { get; set; }

    public string AccountHashedId  { get; set; }

    public DateTime AvailableFrom { get; set; }
}

public class ChangeHistoryViewModel
{
    public DateTime AppliedDate { get; set; }

    public string Description { get; set; }

    public LearningChangeType ChangeType { get; set; }

    public Guid Id { get; set; }
}

public enum LearningChangeType : byte
{
    [Description("Auto approved")]
    AutoApproved = 0,

    [Description("Declined")]
    Rejected = 1,

    [Description("Approved")]
    EmployerApproved = 2,

    [Description("Declined")]
    EmployerRejected = 3,

    [Description("Manual update")]
    ManualUpdate = 4
}