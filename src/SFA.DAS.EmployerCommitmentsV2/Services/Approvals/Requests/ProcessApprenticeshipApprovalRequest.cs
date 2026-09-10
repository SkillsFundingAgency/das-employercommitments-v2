using System;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

public class ProcessApprenticeshipApprovalRequest : SaveDataRequest
{
    public bool ApplyChanges { get; set; }
    public long AccountId { get; set; }
}
