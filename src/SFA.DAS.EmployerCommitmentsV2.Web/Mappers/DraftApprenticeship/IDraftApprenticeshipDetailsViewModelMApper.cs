using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class IDraftApprenticeshipDetailsViewModelMapper : IMapper<DetailsRequest, IDraftApprenticeshipViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IModelMapper _modelMapper;

    public IDraftApprenticeshipDetailsViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IModelMapper modelMapper)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _modelMapper = modelMapper;
    }

    public async Task<IDraftApprenticeshipViewModel> Map(DetailsRequest source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

        if (cohort.WithParty == Party.Employer)
            return await MapToEditModel(source, cohort);

        return await MapToViewModel(source, cohort);
    }

    private async Task<EditDraftApprenticeshipViewModel> MapToEditModel(DetailsRequest source, GetCohortResponse cohort)
    {
        return (EditDraftApprenticeshipViewModel)await _modelMapper.Map<IDraftApprenticeshipViewModel>(new EditDraftApprenticeshipRequest
        {
            Cohort = cohort,
            Request = source,
        });
    }

    private async Task<IDraftApprenticeshipViewModel> MapToViewModel(DetailsRequest source, GetCohortResponse cohort)
    {
        return await _modelMapper.Map<IDraftApprenticeshipViewModel>(new ViewDraftApprenticeshipRequest
        {
            Cohort = cohort,
            Request = source
        });
    }
}