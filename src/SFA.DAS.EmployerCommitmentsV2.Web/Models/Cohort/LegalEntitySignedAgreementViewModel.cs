using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class LegalEntitySignedAgreementViewModel
    {
        public string HashedAccountId { get; set; }
        public string TransferConnectionCode { get; set; }
        public string LegalEntityCode { get; set; }
        public string CohortRef { get; set; }
        public bool HasSignedMinimumRequiredAgreementVersion { get; set; }
        public string LegalEntityName { get; set; }
        public bool CanContinueAnyway { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
    }
}
