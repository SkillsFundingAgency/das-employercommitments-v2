using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public class AddDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public long AccountId { get; set; }
        public string AccountHashedId { get; set; }

        public long AccountLegalEntityId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public string TransferSenderHashedId { get; set; }
        public bool AutoCreatedReservation { get; set; }
    }
}
