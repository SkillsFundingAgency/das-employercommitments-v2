using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using Agreement = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.Agreement;
using AgreementModel = SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort.Agreement;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectLegalEntityRequestToSelectLegalEntityViewModelMapper(
    IApprovalsApiClient apiClient,
    IEncodingService encodingService) : IMapper<SelectLegalEntityRequest, SelectLegalEntityViewModel>
{
    public async Task<SelectLegalEntityViewModel> Map(SelectLegalEntityRequest source)
    {
        var cohortRef = string.IsNullOrWhiteSpace(source.cohortRef)
            ? Guid.NewGuid().ToString().ToUpper()
            : source.cohortRef;
        var accountId = encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
        var legalEntities = await apiClient.GetLegalEntitiesForAccount(cohortRef, accountId);

        return new SelectLegalEntityViewModel {
            AccountHashedId = source.AccountHashedId,
            TransferConnectionCode = source.transferConnectionCode,
            LegalEntities = legalEntities.AccountLegalEntities.ConvertAll(MapToLegalEntityVm),
            CohortRef = cohortRef,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId
        };
    }

    private LegalEntity MapToLegalEntityVm(AccountLegalEntity input)
    {
        return new LegalEntity
        {
            Name = input.Name,
            RegisteredAddress = input.Address,
            Id = input.LegalEntityId,
            AccountLegalEntityPublicHashedId = input.AccountLegalEntityPublicHashedId,
            Agreements = input.Agreements.ConvertAll(MapToAgreementVm)
        };
    }

    private AgreementModel MapToAgreementVm(Agreement input)
    {
        return new AgreementModel
        {
            Id = input.Id,
            Status = input.Status
        };
    }
}