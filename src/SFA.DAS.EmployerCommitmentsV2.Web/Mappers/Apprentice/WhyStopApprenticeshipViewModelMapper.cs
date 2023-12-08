using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class WhyStopApprenticeshipViewModelMapper:IMapper<WhyStopApprenticeshipRequest,WhyStopApprenticeshipViewModel>
{
    public Task<WhyStopApprenticeshipViewModel> Map(WhyStopApprenticeshipRequest source)
    {
        var result = new WhyStopApprenticeshipViewModel
        {
            ApprenticeshipId=source.ApprenticeshipId,
            ApprenticeshipHashedId=source.ApprenticeshipHashedId,
            AccountHashedId=source.AccountHashedId
        };
        return Task.FromResult(result);
    }
}