using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class WhenValidatingSelectProviderViewModel
    {
        // check null
        // check non numeric
        // less than 1
        // greater than 1

        [Test, MoqAutoData]
        public void AndTheProviderIdIsDefault_ThenReturnsInvalid(
            SelectProviderViewModel viewModel,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = "0";

            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);

        }

        [Test, MoqAutoData]
        public void AndTheEmployerLegalEntityPublicHashedIdIsEmpty_ThenReturnsInvalid(
            SelectProviderViewModel viewModel,
            long providerId,
            SelectProviderViewModelValidator validator)
        {
            viewModel.ProviderId = providerId.ToString();
            viewModel.EmployerAccountLegalEntityPublicHashedId = string.Empty;

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
            viewModel.EmployerAccountLegalEntityPublicHashedId = "  ";

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
            viewModel.EmployerAccountLegalEntityPublicHashedId = someString;

            var result = validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }
    }
}
