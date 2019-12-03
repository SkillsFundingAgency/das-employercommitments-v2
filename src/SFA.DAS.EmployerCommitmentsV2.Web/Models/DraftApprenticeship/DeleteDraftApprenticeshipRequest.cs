using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public class DeleteApprenticeshipRequest : IAuthorizationContextModel
    {

        public string AccountHashedId { get; set; }
        public string CohortReference { get; set; }
        public long CohortId { get; set; }
        public long AccountId { get; set; }
        public long DraftApprenticeshipId { get; set; }
        public string DraftApprenticeshipHashedId { get; set; }
        public DeleteDraftApprenticeshipOrigin Origin { get; set; }
    }

    public enum DeleteDraftApprenticeshipOrigin
    {
        CohortDetails,
        EditDraftApprenticeship
    }
}
