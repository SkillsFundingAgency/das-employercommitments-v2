using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ApprenticeshipHasNotEndedRequestToViewModelMapper : IMapper<ApprenticeshipNotEndedRequest, ApprenticeshipNotEndedViewModel>
{
    public Task<ApprenticeshipNotEndedViewModel> Map(ApprenticeshipNotEndedRequest source)
    {
        var result = new ApprenticeshipNotEndedViewModel
        {
            ApprenticeshipId = source.ApprenticeshipId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            AccountHashedId = source.AccountHashedId
        };

        return Task.FromResult(result);
    }
}