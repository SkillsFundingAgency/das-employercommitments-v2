using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddApprenticeshipCacheModelToAgreementNotSignedViewModelMapper(
    IEmployerAccountsService employerAccountsService,
    IEncodingService encodingService)
    : IMapper<AddApprenticeshipCacheModel, AgreementNotSignedViewModel>
{
    public async Task<AgreementNotSignedViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var accountId = encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);

        var account = await employerAccountsService.GetAccount(accountId);

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
            CanContinueAnyway = (account.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy),
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey
        };
    }
}