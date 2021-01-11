using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhoWillEnterTheDetailsViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public string ProviderName { get; set; }
        public long ProviderId { get; set; }
        public bool? EmployerWillAdd { get; set; }
    }
}
