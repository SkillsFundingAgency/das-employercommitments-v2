using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.LocalDevRegistry
{
    public class LocalAccountApiClient : IAccountApiClient
    {
        private readonly HttpClient _httpClient;

        public LocalAccountApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("https://sfa-stub-??????.herokuapp.com/") };
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

        public async Task<ICollection<ResourceViewModel>> GetLegalEntitiesConnectedToAccount(string accountId)
        {
            var le = new ResourceViewModel { Id = "645", Href = "X9JE72" };
            var list = new Collection<ResourceViewModel>();
            list.Add(le);

            return await Task.FromResult(list);
        }

        public Task<LegalEntityViewModel> GetLegalEntity(string accountId, long id)
        {
            return Task.FromResult(new LegalEntityViewModel
            {
                Name = "Rapid Logistics Co Ltd",
                Address = "xxxx",
                Source = "Source",
                Agreements = new List<AgreementViewModel>{ new AgreementViewModel
                {
                    Id = 123,
                    SignedDate = DateTime.Now,
                    SignedByName = "SignedBy",
                    Status = EmployerAgreementStatus.Signed,
                    TemplateVersionNumber = 10
                }},
                Code = "X9JE72",
                LegalEntityId = id,
                AccountLegalEntityPublicHashedId = "X9JE72"
            });
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
            return await Task.FromResult(new Collection<TransferConnectionViewModel>());
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
