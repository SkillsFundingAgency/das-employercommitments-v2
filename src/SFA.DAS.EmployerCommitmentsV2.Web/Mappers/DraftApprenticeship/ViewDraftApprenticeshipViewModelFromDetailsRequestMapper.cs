using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class ViewDraftApprenticeshipViewModelFromDetailsRequestMapper : IMapper<DetailsRequest, ViewDraftApprenticeshipViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IModelMapper _modelMapper;

    public ViewDraftApprenticeshipViewModelFromDetailsRequestMapper(ICommitmentsApiClient commitmentsApiClient, IModelMapper modelMapper)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _modelMapper = modelMapper;
    }

    public async Task<ViewDraftApprenticeshipViewModel> Map(DetailsRequest source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);
        return (ViewDraftApprenticeshipViewModel)await _modelMapper.Map<IDraftApprenticeshipViewModel>(new ViewDraftApprenticeshipRequest
        {
            Cohort = cohort,
            Request = source
        });
    }
}