using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Commitments.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsViewModel
    {
        public string AccountHashedId { get; set; }
        public Party WithParty { get; set; }
        public string CohortReference { get; set; }
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
        public IReadOnlyCollection<CohortDraftApprenticeshipViewModel> DraftApprenticeships { get; set; }
        public int TotalCost => DraftApprenticeships.Sum(da => da.Cost ?? 0);
        public string DisplayTotalCost => TotalCost.ToGdsCostFormat();
    }
}
