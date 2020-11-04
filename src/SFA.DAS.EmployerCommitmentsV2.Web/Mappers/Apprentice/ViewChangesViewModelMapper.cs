using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ViewChangesViewModelMapper : IMapper<ViewChangesRequest, ViewChangesViewModel>
    {
        private readonly ICommitmentsApiClient _client;

        public ViewChangesViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<ViewChangesViewModel> Map(ViewChangesRequest source)
        {
            var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

            var result = new ViewChangesViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}"
            };

            return result;
        }
    }
}
