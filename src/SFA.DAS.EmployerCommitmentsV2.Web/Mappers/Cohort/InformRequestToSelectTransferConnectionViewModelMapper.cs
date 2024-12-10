using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class InformRequestToSelectTransferConnectionViewModelMapper : IMapper<InformRequest, SelectTransferConnectionViewModel>
{
    private readonly IApprovalsApiClient _approvalsApiClient;
    private readonly ILogger<InformRequestToSelectTransferConnectionViewModelMapper> _logger;

    public InformRequestToSelectTransferConnectionViewModelMapper(IApprovalsApiClient approvalsApiClient, ILogger<InformRequestToSelectTransferConnectionViewModelMapper> logger)
    {
        _approvalsApiClient = approvalsApiClient;
        _logger = logger;
    }

    public async Task<SelectTransferConnectionViewModel> Map(InformRequest source)
    {
        _logger.LogInformation("Getting Direct Transfers for Account hash {0} id {1}", source.AccountHashedId, source.AccountId);

        var result = await _approvalsApiClient.GetSelectDirectTransferConnection(source.AccountId);

        return new SelectTransferConnectionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            IsLevyAccount = result.IsLevyAccount,
            TransferConnections = result.TransferConnections == null ? new List<TransferConnection>() : result.TransferConnections.ToList()
        };
    }
}