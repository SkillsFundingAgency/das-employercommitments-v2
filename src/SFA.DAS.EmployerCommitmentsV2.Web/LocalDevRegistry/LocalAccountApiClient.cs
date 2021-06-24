using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Stubs;

namespace SFA.DAS.EmployerCommitmentsV2.Web.LocalDevRegistry
{
    public class LocalAccountApiClient : IAccountApiClient
    {
        private readonly HttpClient _httpClient;

        public LocalAccountApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("https://das-commitments-stub-accapi.herokuapp.com/") };
        }

        public Task<AccountDetailViewModel> GetAccount(string hashedAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDetailViewModel> GetAccount(long accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TeamMemberViewModel>> GetAccountUsers(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TeamMemberViewModel>> GetAccountUsers(long accountId)
        {
            throw new NotImplementedException();
        }

        public Task<EmployerAgreementView> GetEmployerAgreement(string accountId, string legalEntityId, string agreementId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ResourceViewModel>> GetLegalEntitiesConnectedToAccount(string hashedAccountId)
        {
            var response = await _httpClient.GetAsync($"api/accounts/{hashedAccountId}/legalentities", CancellationToken.None).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return new Collection<ResourceViewModel>();
            }
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var ales = JsonConvert.DeserializeObject<Collection<ResourceViewModel>>(content);
            return ales;
        }

        public async Task<LegalEntityViewModel> GetLegalEntity(string hashedAccountId, long id)
        {
            var response = await _httpClient.GetAsync($"api/accounts/{hashedAccountId}/legalentities/{id}", CancellationToken.None).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var ale = JsonConvert.DeserializeObject<LegalEntityViewModel>(content);
            return ale;
        }

        public Task<ICollection<LevyDeclarationViewModel>> GetLevyDeclarations(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedApiResponseViewModel<AccountLegalEntityViewModel>> GetPageOfAccountLegalEntities(int pageNumber = 1, int pageSize = 1000)
        {
            throw new NotImplementedException();
        }

        public Task<PagedApiResponseViewModel<AccountWithBalanceViewModel>> GetPageOfAccounts(int pageNumber = 1, int pageSize = 1000, DateTime? toDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ResourceViewModel>> GetPayeSchemesConnectedToAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetResource<T>(string uri)
        {
            throw new NotImplementedException();
        }

        public Task<StatisticsViewModel> GetStatistics()
        {
            throw new NotImplementedException();
        }

        public Task<TransactionsViewModel> GetTransactions(string accountId, int year, int month)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TransactionSummaryViewModel>> GetTransactionSummary(string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<TransferConnectionViewModel>> GetTransferConnections(string accountHashedId)
        {

            var response = await _httpClient.GetAsync($"api/accounts/{accountHashedId}/transfers/connections", CancellationToken.None).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return new Collection<TransferConnectionViewModel>();
            }
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var connections = JsonConvert.DeserializeObject<Collection<TransferConnectionViewModel>>(content);
            return connections;
        }

        public Task<ICollection<AccountDetailViewModel>> GetUserAccounts(string userId)
        {
            throw new NotImplementedException();
        }

        public Task Ping()
        {
            throw new NotImplementedException();
        }
    }
}
