using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class DataLockRequestChangesViewModelToRejectRequestModelMapper : IMapper<DataLockRequestChangesViewModel, RejectDataLocksRequestChangesRequest>
{
    private readonly IAuthenticationService _authenticationService;

    public DataLockRequestChangesViewModelToRejectRequestModelMapper(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<RejectDataLocksRequestChangesRequest> Map(DataLockRequestChangesViewModel source)
    {
        return await Task.FromResult(new RejectDataLocksRequestChangesRequest
        {
            ApprenticeshipId = source.ApprenticeshipId,
            UserInfo = _authenticationService.UserInfo
        });
    }
}