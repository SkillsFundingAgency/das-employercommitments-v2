using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class WhoWillEnterTheDetailsViewModelMapper : IMapper<ChangeOfProviderRequest, WhoWillEnterTheDetailsViewModel>
    {
        private readonly ICommitmentsApiClient _client;

        public WhoWillEnterTheDetailsViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<WhoWillEnterTheDetailsViewModel> Map(ChangeOfProviderRequest source)
        {
            var provider = await _client.GetProvider(source.ProviderId);

            return new WhoWillEnterTheDetailsViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderName = provider.Name,
                ProviderId = source.ProviderId
            };
        }
    }
}
