using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class SendNewTrainingProviderViewModelValidatorTests : ValidatorTestBase<SendNewTrainingProviderViewModel, SendNewTrainingProviderViewModelValidator>
    {
        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ThenAccountHashedIdIsValidated(string accountHashedId, bool expectedValid)
        {
            var request = new SendNewTrainingProviderViewModel { AccountHashedId = accountHashedId };
            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }
        [TestCase("", false)]
        [TestCase("31", true)]
        [TestCase(" ", false)]
        [TestCase(null, false)]
        public void ThenApprenticeshipHashedIdIsValidated(string apprenticeshipHashedId, bool expectedValid)
        {
            var request = new SendNewTrainingProviderViewModel { ApprenticeshipHashedId = apprenticeshipHashedId };
            AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
        }

        [TestCase(default(long), false)]
        [TestCase(-2342, false)]
        [TestCase(234, true)]
        public void ThenProviderIsValidated(long providerId, bool expectedValid)
        {
            var request = new SendNewTrainingProviderViewModel { ProviderId = providerId };
            AssertValidationResult(x => x.ProviderId, request, expectedValid);
        }

        [TestCase(null, false)]
        [TestCase(true, true)]
        [TestCase(false, true)]
        public void ThenConfirmIsValidated(bool? confirm, bool expectedValid)
        {
            var request = new SendNewTrainingProviderViewModel { Confirm = confirm };
            AssertValidationResult(x => x.Confirm, request, expectedValid);
        }

        [TestCase(default(long), false)]
        [TestCase(-2342, false)]
        [TestCase(234, true)]
        public void ThenApprenticeshipIdIsValidated(long apprenticeshipId, bool expectedValid)
        {
            var request = new SendNewTrainingProviderViewModel { ApprenticeshipId = apprenticeshipId };
            AssertValidationResult(x => x.ApprenticeshipId, request, expectedValid);
        }

        [TestCase(default(long), false)]
        [TestCase(-2342, false)]
        [TestCase(234, true)]
        public void ThenAccountIdIsValidated(long accountId, bool expectedValid)
        {
            var request = new SendNewTrainingProviderViewModel { AccountId = accountId };
            AssertValidationResult(x => x.AccountId, request, expectedValid);
        }
    }
}
