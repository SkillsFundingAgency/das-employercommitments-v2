using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models
{
    public class AddDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }

        public long AccountLegalEntityId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
    }
}
