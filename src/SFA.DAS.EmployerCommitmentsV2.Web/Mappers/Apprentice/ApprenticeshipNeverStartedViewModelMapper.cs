using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using NLog.LayoutRenderers;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ApprenticeshipNeverStartedViewModelMapper : IMapper<ApprenticeshipNeverStartedRequest, ApprenticeshipNeverStartedViewModel>
    {
        private readonly ICommitmentsApiClient _client;

        public ApprenticeshipNeverStartedViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<ApprenticeshipNeverStartedViewModel> Map(ApprenticeshipNeverStartedRequest source)
        {
            var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

            return new ApprenticeshipNeverStartedViewModel
            {
                ApprenticeshipId = source.ApprenticeshipId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId,
                PlannedStartDate = apprenticeship.StartDate.Value,
                IsCoPJourney = false,
                StopMonth = apprenticeship.StartDate.Value.Month,
                StopYear = apprenticeship.StartDate.Value.Year
            };
        }
    }
}
