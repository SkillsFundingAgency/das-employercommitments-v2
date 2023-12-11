using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

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

        var legalEntity = legalEntities.FirstOrDefault(x => x.Id.Equals(source.LegalEntityId));

        if (legalEntity == null)
        {
            throw new Exception($"LegalEntity Agreement does not exist {source.AccountHashedId}");
        }

        var hasSignedMinimumRequiredAgreementVersion = legalEntity.HasSignedMinimumRequiredAgreementVersion(!string.IsNullOrWhiteSpace(source.TransferConnectionCode));

        return new LegalEntitySignedAgreementViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityId = source.LegalEntityId,
            TransferConnectionCode = source.TransferConnectionCode,
            CohortRef = source.CohortRef,
            HasSignedMinimumRequiredAgreementVersion = hasSignedMinimumRequiredAgreementVersion,
            LegalEntityName = legalEntity.Name ?? string.Empty,
            AccountLegalEntityHashedId = legalEntity.AccountLegalEntityPublicHashedId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId
        };
    }
}