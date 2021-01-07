using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class WhatIsTheNewStartDateViewModelMapper : IMapper<WhatIsTheNewStartDateRequest, WhatIsTheNewStartDateViewModel>
    {

        private readonly ICommitmentsApiClient _client;

        public WhatIsTheNewStartDateViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<WhatIsTheNewStartDateViewModel> Map(WhatIsTheNewStartDateRequest source)
        {
            var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);
            
            return new WhatIsTheNewStartDateViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderName = apprenticeship.ProviderName,
                ProviderId = source.ProviderId,
                StopDate = apprenticeship.StopDate.Value
            };
        }
    }
}
