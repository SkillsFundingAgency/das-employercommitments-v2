using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class CancelChangeOfProviderRequestViewModel : ChangeOfProviderBaseViewModel
    {
        public string OldProviderName { get; set; }
        public string ApprenticeName { get; set; }
        public bool? CancelRequest { get; set; }
    }
}
