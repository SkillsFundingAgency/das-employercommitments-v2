using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddApprenticeshipCacheModelToAgreementNotSignedViewModelMapper(
    IApprovalsApiClient client,
    IEncodingService encodingService)
    : IMapper<AddApprenticeshipCacheModel, AgreementNotSignedViewModel>
{
    public async Task<AgreementNotSignedViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var accountId = encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
        var accountStatus = await client.GetAgreementNotSigned(accountId);

        return new AgreementNotSignedViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            CohortRef = source.CohortRef,
            HasSignedMinimumRequiredAgreementVersion = source.HasSignedMinimumRequiredAgreementVersion,
            AccountLegalEntityId = source.AccountLegalEntityId,
            LegalEntityName = source.LegalEntityName,
            TransferConnectionCode = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            CanContinueAnyway = accountStatus.IsLevyAccount,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey
        };
    }
}