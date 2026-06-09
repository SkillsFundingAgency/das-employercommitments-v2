using System;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

public class ChangePaymentsApimRequest : ApimSaveDataRequest
{
    public DateTime? PaymentFreezeDate { get; set; }
    public int? FreezePaymentsReason { get; set; }
}
