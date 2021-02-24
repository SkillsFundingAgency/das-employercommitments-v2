using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class CopConfirmStopRequestToViewModelMapper : IMapper<CopConfirmStopRequest, CopConfirmStopRequestViewModel>
    {
        public Task<CopConfirmStopRequestViewModel> Map(CopConfirmStopRequest source)
        {
            return Task.FromResult(new CopConfirmStopRequestViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId
            });
        }
    }
}
