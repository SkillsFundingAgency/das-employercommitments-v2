namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectTransferConnectionViewModel
{
    public string AccountHashedId { get; set; }

    public string TransferConnectionCode { get; set; }

    public List<TransferConnection> TransferConnections { get; set; }
}

public class TransferConnection
{
    public string TransferConnectionName { get; set; }
    public string TransferConnectionCode { get; set; }
}