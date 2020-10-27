using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class SendNewTrainingProviderViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public long ApprenticeshipId { get; set; }
        public long ProviderId { get; set; }
        public string ApprenticeName { get; set; }
        public string EmployerName { get; set; }
        public string OldProviderName { get; set; }
        public string NewProviderName { get; set; }
        public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
        public bool? Confirm { get; set; }
    }
}
