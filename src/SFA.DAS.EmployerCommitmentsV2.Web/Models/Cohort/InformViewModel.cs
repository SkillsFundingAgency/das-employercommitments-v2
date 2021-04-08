using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class InformViewModel
    {   
        public List<TransferConnection> TransferConnections { get; set; }
    }

    public class TransferConnection
    {
        public string AccountName { get; set; }
        public long AccountId { get; set; }
    }
}
