using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ConfirmNewTrainingProviderViewModelMapper : IMapper<ConfirmNewTrainingProviderRequest, ConfirmNewTrainingProviderViewModel>
    {
        public Task<ConfirmNewTrainingProviderViewModel> Map(ConfirmNewTrainingProviderRequest source)
        {
            var result = new ConfirmNewTrainingProviderViewModel
            {
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId,
                Ukprn = source.Ukprn
            };

            return Task.FromResult(result);
        }
    }
}
