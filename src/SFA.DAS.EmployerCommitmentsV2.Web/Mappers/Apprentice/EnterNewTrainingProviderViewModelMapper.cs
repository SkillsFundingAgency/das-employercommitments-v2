using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EnterNewTrainingProviderViewModelMapper : IMapper<EnterNewTrainingProviderRequest, EnterNewTrainingProviderViewModel>
    {
        public Task<EnterNewTrainingProviderViewModel> Map(EnterNewTrainingProviderRequest source)
        {
            var result = new EnterNewTrainingProviderViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId
            };

            return Task.FromResult(result);
        }
    }
}
