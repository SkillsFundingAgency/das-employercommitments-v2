using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class WhoWillEnterTheDetailsRequestMapper : IMapper<EnterNewTrainingProviderViewModel, WhoWillEnterTheDetailsRequest>
{
    public Task<WhoWillEnterTheDetailsRequest> Map(EnterNewTrainingProviderViewModel source)
    {
        var result = new WhoWillEnterTheDetailsRequest
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ProviderId = source.ProviderId.Value
        };

        return Task.FromResult(result);
    }
}