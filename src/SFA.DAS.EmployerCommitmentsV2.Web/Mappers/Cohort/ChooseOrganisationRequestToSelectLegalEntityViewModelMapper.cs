using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ChooseOrganisationRequestToSelectLegalEntityViewModelMapper : IMapper<ChooseOrganisationRequest, SelectLegalEntityViewModel>
    {
        private readonly IAccountApiClient _accountsApiClient;

        public ChooseOrganisationRequestToSelectLegalEntityViewModelMapper(IAccountApiClient accountApiClient)
        {
            _accountsApiClient = accountApiClient;
        }

        public async Task<SelectLegalEntityViewModel> Map(ChooseOrganisationRequest source)
        {
            var legalEntities = await GetLegalEntitiesForAccount(source.AccountHashedId);

            return new SelectLegalEntityViewModel { 
                    TransferConnectionCode = source.transferConnectionCode, 
                    LegalEntities = legalEntities,                    
                    CohortRef = string.IsNullOrWhiteSpace(source.cohortRef) ? Guid.NewGuid().ToString().ToUpper() : source.cohortRef // TODO : check about cohort ref
            };
        }

        public async Task<List<LegalEntity>> GetLegalEntitiesForAccount(string accountId)
        {
            var listOfEntities = await _accountsApiClient.GetLegalEntitiesConnectedToAccount(accountId);  

            var list = new List<LegalEntity>();

            if (listOfEntities.Count == 0) return list;

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
