using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhoWillEnterTheDetailsViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }

        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public bool? EmployerWillAdd { get; set; }
        public int? NewStartMonth { get; set; }
        public int? NewStartYear { get; set; }
        public int? NewEndMonth { get; set; }
        public int? NewEndYear { get; set; }
        public int? NewPrice { get; set; }

        public bool Edit { get; set; }
    }
}
