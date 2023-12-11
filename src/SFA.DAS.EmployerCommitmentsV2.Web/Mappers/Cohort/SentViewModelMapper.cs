using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SentViewModelMapper :IMapper<SentRequest,SentViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public SentViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<SentViewModel> Map(SentRequest source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

        return new SentViewModel
        {
            AccountHashedId = source.AccountHashedId,
            CohortReference = source.CohortReference,
            LegalEntityName = cohort.LegalEntityName,
            ProviderName = cohort.ProviderName,
            CohortId = source.CohortId
        };
    }
}