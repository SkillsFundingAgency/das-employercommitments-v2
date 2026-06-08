namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

public class ChangePaymentsApimRequest : ApimSaveDataRequest
{
    public bool FreezePayments { get; set; }
    public int? FreezePaymentsReason { get; set; }
}
