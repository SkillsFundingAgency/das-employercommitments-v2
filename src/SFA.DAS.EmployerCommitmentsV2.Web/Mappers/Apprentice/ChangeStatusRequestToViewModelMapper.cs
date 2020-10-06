using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeStatusRequestToViewModelMapper : IMapper<ChangeStatusRequest, ChangeStatusRequestViewModel>
    {
        public Task<ChangeStatusRequestViewModel> Map(ChangeStatusRequest source)
        {
            var result = new ChangeStatusRequestViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            };

            return Task.FromResult(result);
        }
    }
}
