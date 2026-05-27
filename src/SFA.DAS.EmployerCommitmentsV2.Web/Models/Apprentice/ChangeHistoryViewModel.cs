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
    [Description("Change auto approved")]
    AutoApproved = 0,

    [Description("Change rejected")]
    Rejected = 1,

    [Description("Change employer approved")]
    EmployerApproved = 2,

    [Description("Change employer rejected")]
    EmployerRejected = 3,

    [Description("Manual update")]
    ManualUpdate = 4
}