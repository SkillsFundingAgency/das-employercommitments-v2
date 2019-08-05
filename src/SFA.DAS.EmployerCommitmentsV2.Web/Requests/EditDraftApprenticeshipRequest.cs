using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Requests
{
    public class EditDraftApprenticeshipRequest : IAuthorizationContextModel
    {
        public long AccountId { get; set; }
        public string AccountHashedId { get; set; }
        public long CohortId { get; set; }
        public string CohortReference { get; set; }
        public long DraftApprenticeshipId { get; set; }
        public string DraftApprenticeshipHashedId { get; set; }
    }
}