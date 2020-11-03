using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ChangeProviderRequestedConfirmationViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public long ApprenticeshipId { get; set; }

        public string ProviderName { get; set; }
        public string ApprenticeName { get; set; }
    }
}
