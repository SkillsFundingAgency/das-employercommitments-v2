using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeProviderRequestedConfirmationViewModelMapper : IMapper<ChangeProviderRequestedConfirmationRequest, ChangeProviderRequestedConfirmationViewModel>
    {
        public Task<ChangeProviderRequestedConfirmationViewModel> Map(ChangeProviderRequestedConfirmationRequest source)
        {
            var result = new ChangeProviderRequestedConfirmationViewModel
            {
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId
            };

            return Task.FromResult(result);
        }
    }
}
