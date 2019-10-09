using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ViewEmployerAgreementMapper : IMapper<DetailsViewModel, ViewEmployerAgreementRequest>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;

        public ViewEmployerAgreementMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
        {
            _encodingService = encodingService;
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<ViewEmployerAgreementRequest> Map(DetailsViewModel source)
        {
            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);
            return new ViewEmployerAgreementRequest
            {
                AccountLegalEntityHashedId = _encodingService.Encode(cohort.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
                AccountHashedId = source.AccountHashedId
            };
        }
    }
}