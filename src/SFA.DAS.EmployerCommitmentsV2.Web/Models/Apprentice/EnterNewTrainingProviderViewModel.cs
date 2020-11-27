using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.Authorization.ModelBinding;
using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class EnterNewTrainingProviderViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public long Ukprn { get; set; }
        public long CurrentProviderId { get; set; }
        public List<Provider> Providers { get; set; }
        public long ApprenticeshipId { get; set; }
    }
}
