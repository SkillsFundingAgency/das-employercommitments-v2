using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using Agreement = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.Agreement;
using AgreementModel = SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort.Agreement;
using LegalEntity = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.LegalEntity;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectLegalEntityRequestToSelectLegalEntityViewModelMapper(
    IApprovalsApiClient apiClient,
    IEncodingService encodingService) : IMapper<OG_CacheModel, SelectLegalEntityViewModel>
{
    public async Task<SelectLegalEntityViewModel> Map(OG_CacheModel source)
    {
        var cohortRef = string.IsNullOrWhiteSpace(source.CohortRef)
            ? Guid.NewGuid().ToString().ToUpper()
            : source.CohortRef;
        var accountId = encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
        var legalEntities = await apiClient.GetLegalEntitiesForAccount(cohortRef, accountId);

        return new SelectLegalEntityViewModel {
            AccountHashedId = source.AccountHashedId,
            TransferConnectionCode = source.TransferConnectionCode,
            LegalEntities = legalEntities.LegalEntities.ConvertAll(MapToLegalEntityVm),
            CohortRef = cohortRef,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            OG_CacheKey = source.CacheKey
        };
    }

    private Models.Cohort.LegalEntity MapToLegalEntityVm(LegalEntity input)
    {
        return new Models.Cohort.LegalEntity
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
            Status = input.Status,
            TemplateVersionNumber = input.TemplateVersionNumber
        };
    }
}