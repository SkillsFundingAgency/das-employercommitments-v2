using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.Authorization.ModelBinding;
using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class EnterNewTrainingProviderViewModel : IAuthorizationContextModel
    {
        public long CurrentProviderId { get; set; }
        public List<Provider> Providers { get; set; }

        // Move to base viewModel - will validators still work?
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
