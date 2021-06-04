using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{
    public class EmployerAccountsService : IEmployerAccountsService
    {
        private readonly IAccountApiClient _accountsApiClient;

        public EmployerAccountsService(IAccountApiClient accountApiClient)
        {
            _accountsApiClient = accountApiClient;
        }        

        public async Task<List<LegalEntity>> GetLegalEntitiesForAccount(string accountId)
        {
            var listOfEntities = await _accountsApiClient.GetLegalEntitiesConnectedToAccount(accountId);

            var list = new List<LegalEntity>();
            if (listOfEntities.Count == 0) return list;

            list.AddRange(listOfEntities.Select(async entity => await _accountsApiClient.GetLegalEntity(accountId, Convert.ToInt64(entity.Id))).Select(legalEntityViewModel => new LegalEntity
            {
                Name = legalEntityViewModel.Result.Name,
                RegisteredAddress = legalEntityViewModel.Result.Address,
                Source = legalEntityViewModel.Result.SourceNumeric,
                Agreements =
                         legalEntityViewModel.Result.Agreements.Select(agreementSource => new Agreement
                         {
                             Id = agreementSource.Id,
                             SignedDate = agreementSource.SignedDate,
                             SignedByName = agreementSource.SignedByName,
                             Status = (EmployerAgreementStatus)agreementSource.Status,
                             TemplateVersionNumber = agreementSource.TemplateVersionNumber
                         }).ToList(),
                Code = legalEntityViewModel.Result.Code,
                Id = legalEntityViewModel.Result.LegalEntityId,
                AccountLegalEntityPublicHashedId = legalEntityViewModel.Result.AccountLegalEntityPublicHashedId
            }));
            return list;

            //foreach (var entity in listOfEntities)
            //{
            //    var legalEntityViewModel = await _accountsApiClient.GetLegalEntity(accountId, Convert.ToInt64(entity.Id));

            //    list.Add(new LegalEntity
            //    {
            //        Name = legalEntityViewModel.Name,
            //        RegisteredAddress = legalEntityViewModel.Address,
            //        Source = legalEntityViewModel.SourceNumeric,
            //        Agreements =
            //            legalEntityViewModel.Agreements.Select(agreementSource => new Agreement
            //            {
            //                Id = agreementSource.Id,
            //                SignedDate = agreementSource.SignedDate,
            //                SignedByName = agreementSource.SignedByName,
            //                Status = (EmployerAgreementStatus)agreementSource.Status,
            //                TemplateVersionNumber = agreementSource.TemplateVersionNumber
            //            }).ToList(),
            //        Code = legalEntityViewModel.Code,
            //        Id = legalEntityViewModel.LegalEntityId,
            //        AccountLegalEntityPublicHashedId = legalEntityViewModel.AccountLegalEntityPublicHashedId
            //    });
            //}
            //return list;
        }
    }
}
