using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class WhenValidatingSelectProviderViewModel
    { 
        [Test, MoqAutoData]
        public void AndTheProviderIdIsNull_ThenReturnsInvalid(
            SelectProviderViewModel viewModel,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = null;
            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void AndTheProviderIdIsNonNumeric_ThenReturnsInvalid(
            SelectProviderViewModel viewModel,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = "abcdefghijklmnopqrstuvwxyz*!£$%^&*()_+¬`=-][#';/.,}{~@:?><";
            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void AndTheProviderIdIsLessThanOne_ThenReturnsInvalid(
            SelectProviderViewModel viewModel,
            long providerId,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = $"-{Convert.ToString(providerId)}";
            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void AndTheProviderIdIsAboveLongMaxValue_ThenReturnsInvalid(
            SelectProviderViewModel viewModel,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = Double.MaxValue.ToString() ;
            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void AndTheProviderIdIsGreaterThanOne_ThenReturnsValid(
            SelectProviderViewModel viewModel,
            long providerId,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = Convert.ToString(providerId);

            var result = validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }

        [Test, MoqAutoData]
        public void AndProviderIdIsInvalid_ThenCorrectValidationMessageShown(
            SelectProviderViewModel viewModel,
            string invalidId,
            SelectProviderViewModelValidator validator)
        {
            var expectedMessage = "Check UK Provider Reference Number";
            viewModel.ProviderId = invalidId;

            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any(x => x.ErrorMessage == expectedMessage));
        }

        [Test, MoqAutoData]
        public void AndTheEmployerLegalEntityPublicHashedIdIsEmpty_ThenReturnsInvalid(
            SelectProviderViewModel viewModel,
            long providerId,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = providerId.ToString();
            viewModel.AccountLegalEntityHashedId = string.Empty;

            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void AndTheEmployerLegalEntityPublicHashedIdIsWhiteSpace_ThenReturnsInvalid(
            SelectProviderViewModel viewModel,
            long providerId,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = providerId.ToString();
            viewModel.AccountLegalEntityHashedId = "  ";

            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void AndViewModelIsValid_ThenReturnsValid(
            SelectProviderViewModel viewModel,
            long providerId,
            string someString,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = providerId.ToString();
            viewModel.AccountLegalEntityHashedId = someString;

            var result = validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }
    }
}
