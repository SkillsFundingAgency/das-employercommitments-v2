using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class InformRequestToSelectTransferConnectionViewModelMapper : IMapper<InformRequest, SelectTransferConnectionViewModel>
    {
        private readonly IAccountApiClient _accountsApiClient;
        private readonly IEncodingService _encodingService;

        public InformRequestToSelectTransferConnectionViewModelMapper(IAccountApiClient accountApiClient, IEncodingService encodingService)
        {
            _accountsApiClient = accountApiClient;
            _encodingService = encodingService;
        }

        public async Task<SelectTransferConnectionViewModel> Map(InformRequest source)
        {
            var result = await GetTransferConnectionsForAccount(source.AccountHashedId);

            return new SelectTransferConnectionViewModel
            {
                AccountHashedId = source.AccountHashedId,
                TransferConnections = result
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
                TransferConnectionCode = _encodingService.Encode( x.FundingEmployerAccountId, EncodingType.PublicAccountId),
                TransferConnectionName = x.FundingEmployerAccountName
            }).ToList();
        }

    }
}
