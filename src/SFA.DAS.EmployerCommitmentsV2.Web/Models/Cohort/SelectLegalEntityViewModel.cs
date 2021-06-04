using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class SelectLegalEntityViewModel
    {
        public string AccountHashedId { get; set; }
        public string TransferConnectionCode { get; set; }        
        public string LegalEntityCode { get; set; }
        public string CohortRef { get; set; }
        public IEnumerable<LegalEntity> LegalEntities { get; set; }        
    }
}
