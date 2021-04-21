using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class SelectTransferConnectionViewModel
    {
        public string AccountHashedId { get; set; }
        public List<TransferConnection> TransferConnections { get; set; }
    }

    public class TransferConnection
    {
        public string AccountName { get; set; }
        public long AccountId { get; set; }
    }
}
