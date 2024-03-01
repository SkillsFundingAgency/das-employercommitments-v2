namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;

public class TransferConfirmationViewModel
{
    public string AccountHashedId { get; set; }

    public string TransferApprovalStatus { get; set; }
    public string TransferReceiverName { get; set; }
    public Option? SelectedOption { get; set; }

    public enum Option
    {
        Homepage, TransfersDashboard
    }

}