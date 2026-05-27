using System.ComponentModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ChangeHistoryListViewModel
{
    public List<ChangeHistoryViewModel> ChangeHistoryViews { get; set; }

    public string Name { get; set; }

    public string ApprenticeshipHashedId { get; set; }
}

public class ChangeHistoryViewModel
{
    public DateTime AppliedDate { get; set; }

    public string Description { get; set; }

    public LearningChangeType ChangeType { get; set; }
}

public enum LearningChangeType : byte
{
    [Description("Auto approved")]
    AutoApproved = 0,

    [Description("Rejected")]
    Rejected = 1,

    [Description("Approved")]
    EmployerApproved = 2,

    [Description("Rejected")]
    EmployerRejected = 3,

    [Description("Manual update")]
    ManualUpdate = 4
}