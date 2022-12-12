using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ApprenticeshipHasNotEndedRequestToViewModelMapper:IMapper<ApprenticeshipNotEndedRequest,ApprenticeshipNotEndedViewModel>
    {
        private readonly ICommitmentsApiClient _client;

        public ApprenticeshipHasNotEndedRequestToViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<ApprenticeshipNotEndedViewModel> Map(ApprenticeshipNotEndedRequest source)
        {
            return new ApprenticeshipNotEndedViewModel
            {
                ApprenticeshipId = source.ApprenticeshipId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId
            };
        }
    }
}
