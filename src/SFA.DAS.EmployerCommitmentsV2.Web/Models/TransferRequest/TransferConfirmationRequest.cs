namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;

public class TransferConfirmationRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }

    [FromRoute]
    public string TransferRequestHashedId { get; set; }
    public long TransferRequestId { get; set; }
}