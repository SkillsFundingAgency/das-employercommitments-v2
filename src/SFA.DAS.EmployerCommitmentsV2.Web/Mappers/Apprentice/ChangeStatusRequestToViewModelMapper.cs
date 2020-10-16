using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeStatusRequestToViewModelMapper : IMapper<ChangeStatusRequest, ChangeStatusRequestViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public ChangeStatusRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<ChangeStatusRequestViewModel> Map(ChangeStatusRequest source)
        {
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);
            
            var result = new ChangeStatusRequestViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ApprenticeshipId = source.ApprenticeshipId,
                CurrentStatus = apprenticeship.Status
            };

            return result;
        }
    }
}
