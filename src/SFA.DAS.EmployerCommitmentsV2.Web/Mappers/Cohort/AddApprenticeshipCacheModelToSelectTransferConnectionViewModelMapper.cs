using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddApprenticeshipCacheModelToSelectTransferConnectionViewModelMapper(IAccountApiClient accountApiClient, IEncodingService encodingService) : IMapper<AddApprenticeshipCacheModel, SelectTransferConnectionViewModel>
{  
    public async Task<SelectTransferConnectionViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var result = await GetTransferConnectionsForAccount(source.AccountHashedId);

        return new SelectTransferConnectionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            TransferConnections = result,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey
        };
    }

    public async Task<List<TransferConnection>> GetTransferConnectionsForAccount(string accountHashedId)
    {
        var listOfTransferConnections = await accountApiClient.GetTransferConnections(accountHashedId);

        if (listOfTransferConnections == null)
        {
            return [];
        }

        return listOfTransferConnections.Select(x => new TransferConnection
        {
            TransferConnectionCode = encodingService.Encode(x.FundingEmployerAccountId, EncodingType.PublicAccountId),
            TransferConnectionName = x.FundingEmployerAccountName
        }).ToList();
    }
}