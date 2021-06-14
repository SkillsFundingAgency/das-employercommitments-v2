using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class DataLockRequestChangesViewModelToAcceptRequestModelMapper : IMapper<DataLockRequestChangesViewModel, AcceptDataLocksRequestChangesRequest>
    {
        private readonly IAuthenticationService _authenticationService;

        public DataLockRequestChangesViewModelToAcceptRequestModelMapper(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AcceptDataLocksRequestChangesRequest> Map(DataLockRequestChangesViewModel source)
        {
            return await Task.FromResult(new AcceptDataLocksRequestChangesRequest
            {
                ApprenticeshipId = source.ApprenticeshipId,
                UserInfo = _authenticationService.UserInfo
            });
        }
    }
}
