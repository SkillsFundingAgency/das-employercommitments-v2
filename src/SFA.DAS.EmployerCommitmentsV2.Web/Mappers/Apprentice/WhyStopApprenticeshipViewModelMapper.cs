using Microsoft.AspNetCore.Mvc;
using NLog.LayoutRenderers;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class WhyStopApprenticeshipViewModelMapper :IMapper<WhyStopApprenticeshipRequest,WhyStopApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _client;

        public WhyStopApprenticeshipViewModelMapper
            (ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<WhyStopApprenticeshipViewModel> Map(WhyStopApprenticeshipRequest source)
        {
            var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

            return new WhyStopApprenticeshipViewModel
            {
                ApprenticeshipId=source.ApprenticeshipId,
                ApprenticeshipHashedId=source.ApprenticeshipHashedId,
                AccountHashedId=source.AccountHashedId,
               // CurrentStatus= apprenticeship.Status
            };
        }
    }
}
