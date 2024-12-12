namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectFundingRequest : IndexRequest
{
    public string TransferSenderId { get; set; }

    [FromQuery]
    public string EncodedPledgeApplicationId { get; set; }
}