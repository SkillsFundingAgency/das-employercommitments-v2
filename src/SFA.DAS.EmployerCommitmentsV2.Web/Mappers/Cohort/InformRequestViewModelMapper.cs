using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class InformRequestViewModelMapper : IMapper<InformRequest, InformViewModel>
    {
        private readonly IAccountApiClient _accountsApiClient;

        public InformRequestViewModelMapper(IAccountApiClient accountApiClient)
        {
            _accountsApiClient = accountApiClient;
        }

        public async Task<InformViewModel> Map(InformRequest source)
        {
            var result = await GetTransferConnectionsForAccount(source.AccountHashedId);

            return new InformViewModel
            {
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
                AccountId = x.FundingEmployerAccountId,
                AccountName = x.FundingEmployerAccountName
            }).ToList();
        }

    }
}
