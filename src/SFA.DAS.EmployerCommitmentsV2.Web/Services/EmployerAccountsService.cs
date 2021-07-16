using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dasync.Collections;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{
    public class EmployerAccountsService : IEmployerAccountsService
    {
        private readonly IAccountApiClient _accountsApiClient;

        // Set at 50 to limit the number of concurrent api calls
        private const int MaxConcurrentThreads = 50;

        public EmployerAccountsService(IAccountApiClient accountApiClient)
        {
            _accountsApiClient = accountApiClient;
        }

        public async Task<Account> GetAccount(long accountId)
        {
            var account = await _accountsApiClient.GetAccount(accountId);
            var response = new Account
            {
                Id = account.AccountId,
                ApprenticeshipEmployerType = account.ApprenticeshipEmployerType.ToEnum<ApprenticeshipEmployerType>()
            };

            return response;
        }

        public async Task<List<LegalEntity>> GetLegalEntitiesForAccount(string accountId)
        {
            var listOfEntities = await _accountsApiClient.GetLegalEntitiesConnectedToAccount(accountId);
            if (listOfEntities.Count == 0) return new List<LegalEntity>();

            var bag = new ConcurrentBag<LegalEntity>();

            await listOfEntities.ParallelForEachAsync(async entity =>
                {
                    var legalEntityViewModel =
                        await _accountsApiClient.GetLegalEntity(accountId, Convert.ToInt64(entity.Id));

                    bag.Add(new LegalEntity
                    {
                        Name = legalEntityViewModel.Name,
                        RegisteredAddress = legalEntityViewModel.Address,
                        Source = legalEntityViewModel.SourceNumeric,
                        Agreements =
                            legalEntityViewModel.Agreements.Select(agreementSource => new Agreement
                            {
                                Id = agreementSource.Id,
                                SignedDate = agreementSource.SignedDate,
                                SignedByName = agreementSource.SignedByName,
                                Status = (EmployerAgreementStatus) agreementSource.Status,
                                TemplateVersionNumber = agreementSource.TemplateVersionNumber
                            }).ToList(),
                        Code = legalEntityViewModel.Code,
                        Id = legalEntityViewModel.LegalEntityId,
                        AccountLegalEntityPublicHashedId = legalEntityViewModel.AccountLegalEntityPublicHashedId
                    });
                },
                MaxConcurrentThreads
            );

            return bag.OrderBy(le => le.Id).ToList();
        }
    }
}
