using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ViewChangesViewModelMapper : IMapper<ViewChangesRequest, ViewChangesViewModel>
    {
        public Task<ViewChangesViewModel> Map(ViewChangesRequest source)
        {
            var result = new ViewChangesViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId
            };

            return Task.FromResult(result);
        }
    }
}
