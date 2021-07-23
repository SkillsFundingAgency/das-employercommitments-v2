using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Services;
using SFA.DAS.EmployerCommitmentsV2.Features;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class InformRequestToInformViewModelMapper : IMapper<InformRequest, InformViewModel>
    {
        private readonly IAuthorizationService _authorizationService;

        public InformRequestToInformViewModelMapper(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public async Task<InformViewModel> Map(InformRequest source)
        {
            return new InformViewModel
            {
                AccountHashedId = source.AccountHashedId,
                HasApprenticeEmail = await _authorizationService.IsAuthorizedAsync(EmployerFeature.ApprenticeEmail)
            };
        }
    }
}
