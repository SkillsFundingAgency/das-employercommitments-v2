using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class ViewDraftApprenticeshipViewModelFromDetailsRequestMapper : IMapper<DetailsRequest, ViewDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IModelMapper _modelMapper;
        private readonly IEncodingService _encodingService;

        public ViewDraftApprenticeshipViewModelFromDetailsRequestMapper(ICommitmentsApiClient commitmentsApiClient, IModelMapper modelMapper, IEncodingService encodingService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _modelMapper = modelMapper;
            _encodingService = encodingService;
        }

        public async Task<ViewDraftApprenticeshipViewModel> Map(DetailsRequest source)
        {
            var cohortId = _encodingService.Decode(source.CohortReference, EncodingType.CohortReference);
            var cohort = await _commitmentsApiClient.GetCohort(cohortId);
            return (ViewDraftApprenticeshipViewModel)await _modelMapper.Map<IDraftApprenticeshipViewModel>(new ViewDraftApprenticeshipRequest
            {
                Cohort = cohort,
                Request = source
            });
        }
    }
}