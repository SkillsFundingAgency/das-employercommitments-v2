using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class WhatIsTheNewStartDateViewModelMapper : IMapper<ChangeOfProviderRequest, WhatIsTheNewStartDateViewModel>
    {

        private readonly ICommitmentsApiClient _client;

        public WhatIsTheNewStartDateViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<WhatIsTheNewStartDateViewModel> Map(ChangeOfProviderRequest source)
        {
            var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId.Value);
            
            return new WhatIsTheNewStartDateViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderName = source.ProviderName,
                ProviderId = source.ProviderId,
                EmployerWillAdd = source.EmployerWillAdd,
                StopDate = apprenticeship.StopDate.Value,
                NewStartMonth = source.NewStartMonth,
                NewStartYear = source.NewStartYear,
                NewEndMonth = source.NewEndMonth,
                NewEndYear = source.NewEndYear,
                NewPrice = source.NewPrice,
                Edit = source.Edit ?? false
            };
        }
    }
}
