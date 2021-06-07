using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class SelectedLegalEntityToSignedAgreementViewModelMapper : IMapper<SelectLegalEntityViewModel, LegalEntitySignedAgreementViewModel>
    {        
        private readonly IEmployerAccountsService _employerAccountsService;

        public SelectedLegalEntityToSignedAgreementViewModelMapper(IEmployerAccountsService employerAccountsService)
        {            
            _employerAccountsService = employerAccountsService;
        }

        public async Task<LegalEntitySignedAgreementViewModel> Map(SelectLegalEntityViewModel source)
        {
            var legalEntities = await _employerAccountsService.GetLegalEntitiesForAccount(source.AccountHashedId);

            var legalEntity = legalEntities.FirstOrDefault(
                   c => c.Code.Equals(source.LegalEntityCode, StringComparison.CurrentCultureIgnoreCase));

            if (legalEntity == null)
            {
                throw new Exception($"LegalEntity Agreement does not exist {source.AccountHashedId}");
            }

            var hasSignedMinimumRequiredAgreementVersion = legalEntity.HasSignedMinimumRequiredAgreementVersion(!string.IsNullOrWhiteSpace(source.TransferConnectionCode));

            return new LegalEntitySignedAgreementViewModel
            {
                HashedAccountId = source.AccountHashedId,
                LegalEntityCode = source.LegalEntityCode,
                TransferConnectionCode = source.TransferConnectionCode,
                CohortRef = source.CohortRef,
                HasSignedMinimumRequiredAgreementVersion = hasSignedMinimumRequiredAgreementVersion,
                LegalEntityName = legalEntity.Name ?? string.Empty,
                AccountLegalEntityPublicHashedId = legalEntity.AccountLegalEntityPublicHashedId
            };
        }
    }
}
