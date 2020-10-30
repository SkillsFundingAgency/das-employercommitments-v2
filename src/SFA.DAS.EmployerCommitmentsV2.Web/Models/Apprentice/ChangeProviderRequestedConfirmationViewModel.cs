using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ChangeProviderRequestedConfirmationViewModel
    {
        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
    }
}
