using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class DraftRequestMapper : IMapper<CohortsByAccountRequest, DraftViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IEncodingService _encodingService;
    private readonly IUrlHelper _urlHelper;

    public DraftRequestMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService, IUrlHelper urlHelper)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _encodingService = encodingService;
        _urlHelper = urlHelper;
    }

    public async Task<DraftViewModel> Map(CohortsByAccountRequest source)
    {
        var request = new GetCohortsRequest { AccountId = source.AccountId };
        var apiResponse = await _commitmentsApiClient.GetCohorts(request);

        var cohorts = apiResponse.Cohorts
            .Where(x => x.GetStatus() == CohortStatus.Draft)
            .OrderByDescending(x => x.CreatedOn)
            .Select(x => new DraftCohortSummaryViewModel
            {
                ProviderName = x.ProviderName,
                CohortReference = _encodingService.Encode(x.CohortId, EncodingType.CohortReference),
                NumberOfApprentices = x.NumberOfDraftApprentices,
            }).ToList();

        return new DraftViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipRequestsHeaderViewModel = apiResponse.Cohorts.GetCohortCardLinkViewModel(_urlHelper, source.AccountHashedId, CohortStatus.Draft),
            AccountId = source.AccountId,
            Cohorts = cohorts
        };
    }
}