using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class SelectedLegalEntityToSignedAgreementViewModelMapper : IMapper<SelectLegalEntityViewModel, LegalEntitySignedAgreementViewModel>
    {
        private readonly IAccountApiClient _accountsApiClient;

        public SelectedLegalEntityToSignedAgreementViewModelMapper(IAccountApiClient accountApiClient)
        {
            _accountsApiClient = accountApiClient;
        }

        public async Task<LegalEntitySignedAgreementViewModel> Map(SelectLegalEntityViewModel source)
        {
            var legalEntities = await GetLegalEntitiesForAccount(source.AccountHashedId);

            var legalEntity = legalEntities.FirstOrDefault(
                   c => c.Code.Equals(source.LegalEntityCode, StringComparison.CurrentCultureIgnoreCase));

            if (legalEntity == null)
            {
                throw new Exception($"LegalEntity Agreement does not exist {source.AccountHashedId}");              
            }

            var hasSigned = legalEntity.HasSignedAgreement(!string.IsNullOrWhiteSpace(source.TransferConnectionCode));

            return new LegalEntitySignedAgreementViewModel
            {
                HashedAccountId = source.AccountHashedId,
                LegalEntityCode = source.LegalEntityCode,
                TransferConnectionCode = source.TransferConnectionCode,
                CohortRef = source.CohortRef,
                HasSignedAgreement = hasSigned,
                LegalEntityName = legalEntity.Name ?? string.Empty,
                AccountLegalEntityPublicHashedId = legalEntity.AccountLegalEntityPublicHashedId
            };
        }

        public async Task<List<LegalEntity>> GetLegalEntitiesForAccount(string accountId)
        {
            var listOfEntities = await _accountsApiClient.GetLegalEntitiesConnectedToAccount(accountId);

            var list = new List<LegalEntity>();

            if (listOfEntities.Count == 0)
                return list;

            foreach (var entity in listOfEntities)
            {
                var legalEntityViewModel = await _accountsApiClient.GetLegalEntity(accountId, Convert.ToInt64(entity.Id));

                list.Add(new LegalEntity
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
                            Status = (EmployerAgreementStatus)agreementSource.Status,
                            TemplateVersionNumber = agreementSource.TemplateVersionNumber
                        }).ToList(),
                    Code = legalEntityViewModel.Code,
                    Id = legalEntityViewModel.LegalEntityId,
                    AccountLegalEntityPublicHashedId = legalEntityViewModel.AccountLegalEntityPublicHashedId
                });
            }

            return list;
        }       
    }
}
