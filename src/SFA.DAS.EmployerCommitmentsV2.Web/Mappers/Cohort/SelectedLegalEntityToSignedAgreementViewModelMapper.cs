using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectedLegalEntityToSignedAgreementViewModelMapper(
    IApprovalsApiClient apiClient,
    IEncodingService encodingService)
    : IMapper<SelectLegalEntityViewModel, LegalEntitySignedAgreementViewModel>
{
    public async Task<LegalEntitySignedAgreementViewModel> Map(SelectLegalEntityViewModel source)
    {

        var accountId = encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
        var legalEntities = await apiClient.GetLegalEntitiesForAccount("new", accountId);

        var legalEntity = legalEntities.LegalEntities.FirstOrDefault(x => x.LegalEntityId.Equals(source.LegalEntityId));

        if (legalEntity == null)
        {
            throw new Exception($"LegalEntity Agreement does not exist {source.AccountHashedId}");
        }

        var hasSignedMinimumRequiredAgreementVersion = legalEntity.MapToLegalEntityVm().HasSignedMinimumRequiredAgreementVersion(!string.IsNullOrWhiteSpace(source.TransferConnectionCode));

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