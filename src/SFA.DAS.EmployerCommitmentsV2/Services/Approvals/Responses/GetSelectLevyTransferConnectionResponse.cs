namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetSelectLevyTransferConnectionResponse
{
    public IEnumerable<LevyTransferConnection> Applications { get; set; }
}

public class LevyTransferConnection
{
    public long Id { get; set; }
    public long PledgeId { get; set; }
    public long OpportunityId { get; set; }
    public bool IsNamePublic { get; set; }
    public long SenderEmployerAccountId { get; set; }
    public string SenderEmployerAccountName { get; set; }
    public int TotalAmount { get; set; }
}