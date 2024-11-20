using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class BaseSelectProviderRequestToSelectTransferConnectionViewModelMapper : IMapper<BaseSelectProviderRequest, SelectTransferConnectionViewModel>
{
    private readonly IApprovalsApiClient _approvalsApiClient;

    public BaseSelectProviderRequestToSelectTransferConnectionViewModelMapper(IApprovalsApiClient approvalsApiClient)
    {
        _approvalsApiClient = approvalsApiClient;
    }

    public async Task<SelectTransferConnectionViewModel> Map(BaseSelectProviderRequest source)
    {
        var result = await _approvalsApiClient.GetSelectDirectTransferConnection(source.AccountId);

        return new SelectTransferConnectionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            IsLevyAccount = result.IsLevyAccount,
            TransferConnections = result.TransferConnections.ToList()
        };
    }
}