using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class InformRequestToInformViewModelMapper : IMapper<InformRequest, InformViewModel>
{
    public Task<InformViewModel> Map(InformRequest source)
    {
        return Task.FromResult(new InformViewModel
        {
            AccountHashedId = source.AccountHashedId
        });
    }
}