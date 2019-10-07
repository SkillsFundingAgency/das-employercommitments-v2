using System.Threading.Tasks;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
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

        public Task<ViewEmployerAgreementRequest> Map(DetailsViewModel source)
        {
            var cohort =  _commitmentsApiClient.GetCohort(source.CohortId);

            return Task.FromResult(new ViewEmployerAgreementRequest
            {
                AccountLegalEntityHashedId = _encodingService.Encode(cohort.Result.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
                AccountHashedId = source.AccountHashedId
            });
        }
    }
}