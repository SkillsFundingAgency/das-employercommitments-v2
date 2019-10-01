﻿using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ApprovedViewModel
    {
        public string AccountHashedId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public Party WithParty { get; set; }
        public string CohortReference { get; set; }
        public long CohortId { get; set; }
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
    }
}
