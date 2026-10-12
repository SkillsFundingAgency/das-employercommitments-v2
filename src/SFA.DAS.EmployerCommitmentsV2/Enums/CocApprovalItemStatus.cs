using System.ComponentModel;

namespace SFA.DAS.EmployerCommitmentsV2.Enums;

public enum CocApprovalItemStatus : byte
{
    [Description("Auto approved")]
    AutoApproved = 1,

    [Description("Auto rejected")]
    AutoRejected = 2,

    [Description("Pending")]
    Pending = 3,

    [Description("Employer approved")]
    EmployerApproved = 4,

    [Description("Employer rejected")]
    EmployerRejected = 5
}