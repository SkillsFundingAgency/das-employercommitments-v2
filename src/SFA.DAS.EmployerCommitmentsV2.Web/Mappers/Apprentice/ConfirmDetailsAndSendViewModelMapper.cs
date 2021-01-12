

using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ConfirmDetailsAndSendViewModelMapper : IMapper<EmployerLedChangeOfProviderRequest, ConfirmDetailsAndSendViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public ConfirmDetailsAndSendViewModelMapper(ICommitmentsApiClient client)
        {
            _commitmentsApiClient = client;
        }
        public async Task<ConfirmDetailsAndSendViewModel> Map(EmployerLedChangeOfProviderRequest source)
        {
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId, CancellationToken.None);
            var priceHistory = await _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId, CancellationToken.None);

            var result = new ConfirmDetailsAndSendViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderId = source.ProviderId,
                ProviderName = source.ProviderName,
                NewStartDate = new DateTime(source.NewStartYear.Value, source.NewStartMonth.Value, 1),
                NewEndDate = new DateTime(source.NewEndYear.Value, source.NewEndMonth.Value, 1),
                NewPrice = source.NewPrice,
                ApprenticeFullName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
                ApprenticeshipStopDate = apprenticeship.StopDate,
                CurrentProviderName = apprenticeship.ProviderName,
                CurrentStartDate = apprenticeship.StartDate,
                CurrentEndDate = apprenticeship.EndDate,
                CurrentPrice = (int)priceHistory.PriceEpisodes.FirstOrDefault(p => p.ToDate == null).Cost
            };

            return result;
        }
    }
}
