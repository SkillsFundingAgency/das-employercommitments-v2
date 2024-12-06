namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetSelectDirectTransferConnectionResponse
{
    public bool IsLevyAccount { get; set; }
    public IEnumerable<TransferConnection> TransferConnections { get; set; }
}

public class TransferConnection
{
    public long FundingEmployerAccountId { get; set; }
    public string FundingEmployerHashedAccountId { get; set; }
    public string FundingEmployerPublicHashedAccountId { get; set; }
    public string FundingEmployerAccountName { get; set; }
}