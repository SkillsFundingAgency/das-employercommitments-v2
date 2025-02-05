using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddApprenticeshipCacheModelToSelectLegalEntityViewModelMapper(
    IApprovalsApiClient apiClient,
    IEncodingService encodingService) : IMapper<AddApprenticeshipCacheModel, SelectLegalEntityViewModel>
{
    public async Task<SelectLegalEntityViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var cohortRef = string.IsNullOrWhiteSpace(source.CohortRef)
            ? Guid.NewGuid().ToString().ToUpper()
            : source.CohortRef;
        var accountId = encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
        var legalEntities = await apiClient.GetLegalEntitiesForAccount(cohortRef, accountId);

        return new SelectLegalEntityViewModel
        {
            AccountHashedId = source.AccountHashedId,
            TransferConnectionCode = source.TransferSenderId,
            LegalEntities = legalEntities.LegalEntities.ConvertAll(x=>x.MapToLegalEntityVm()),
            CohortRef = cohortRef,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey
        };
    }
}