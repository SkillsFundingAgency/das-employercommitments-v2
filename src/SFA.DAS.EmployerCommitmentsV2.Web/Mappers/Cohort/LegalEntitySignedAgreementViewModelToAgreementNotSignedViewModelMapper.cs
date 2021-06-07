using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class LegalEntitySignedAgreementViewModelToAgreementNotSignedViewModelMapper : IMapper<LegalEntitySignedAgreementViewModel, AgreementNotSignedViewModel>
    {
        private readonly IEmployerAccountsService _employerAccountsService;
        private readonly IEncodingService _encodingService;

        public LegalEntitySignedAgreementViewModelToAgreementNotSignedViewModelMapper(IEmployerAccountsService employerAccountsService, IEncodingService encodingService)
        {
            _employerAccountsService = employerAccountsService;
            _encodingService = encodingService;
        }

        public async Task<AgreementNotSignedViewModel> Map(LegalEntitySignedAgreementViewModel source)
        {
            var accountId = _encodingService.Decode(source.HashedAccountId, EncodingType.AccountId);

            var account = await _employerAccountsService.GetAccount(accountId);

            return new AgreementNotSignedViewModel
            {
                IsLevyEmployer = account.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy,
                HashedAccountId = source.HashedAccountId,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                CohortRef = source.CohortRef,
                HasSignedAgreement = source.HasSignedAgreement,
                LegalEntityCode = source.LegalEntityCode,
                LegalEntityName = source.LegalEntityName,
                TransferConnectionCode = source.TransferConnectionCode,
                CanContinueAnyway = account.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy
            };
        }
    }
}
