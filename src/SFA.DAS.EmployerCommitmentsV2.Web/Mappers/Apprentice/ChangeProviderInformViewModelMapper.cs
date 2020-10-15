using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeProviderInformViewModelMapper : IMapper<ChangeProviderInformRequest, ChangeProviderInformViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;

        public ChangeProviderInformViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
        }

        public async Task<ChangeProviderInformViewModel> Map(ChangeProviderInformRequest source)
        {
            var apprenticeshipId = _encodingService.Decode(source.ApprenticeshipHashedId, EncodingType.ApprenticeshipId);

            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(apprenticeshipId, CancellationToken.None);

            var result = new ChangeProviderInformViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ApprenticeshipStatus = apprenticeship.Status
            };

            return result;
        }
    }
}
