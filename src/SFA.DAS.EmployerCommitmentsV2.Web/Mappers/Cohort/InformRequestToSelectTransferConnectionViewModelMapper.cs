using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class OG_CacheModelToSelectTransferConnectionViewModelMapper : IMapper<OG_CacheModel, SelectTransferConnectionViewModel>
{
    private readonly IAccountApiClient _accountsApiClient;
    private readonly IEncodingService _encodingService;

    public OG_CacheModelToSelectTransferConnectionViewModelMapper(IAccountApiClient accountApiClient, IEncodingService encodingService)
    {
        _accountsApiClient = accountApiClient;
        _encodingService = encodingService;
    }

    public async Task<SelectTransferConnectionViewModel> Map(OG_CacheModel source)
    {
        var result = await GetTransferConnectionsForAccount(source.AccountHashedId);

        return new SelectTransferConnectionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            TransferConnections = result,
            OG_CacheKey = source.CacheKey
        };
    }

    public async Task<List<TransferConnection>> GetTransferConnectionsForAccount(string accountHashedId)
    {
        var listOfTransferConnections = await _accountsApiClient.GetTransferConnections(accountHashedId);

        if (listOfTransferConnections == null)
        {
            return new List<TransferConnection>();
        }

        return listOfTransferConnections.Select(x => new TransferConnection
        {
            TransferConnectionCode = _encodingService.Encode(x.FundingEmployerAccountId, EncodingType.PublicAccountId),
            TransferConnectionName = x.FundingEmployerAccountName
        }).ToList();
    }

}