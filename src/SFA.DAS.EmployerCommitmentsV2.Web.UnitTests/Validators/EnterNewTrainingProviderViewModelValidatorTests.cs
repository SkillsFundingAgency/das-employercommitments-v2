using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class EnterNewTrainingProviderViewModelValidatorTests
    {
        private const string ExpectedUkprnErrorMessage = "Select a training provider";
        private EnterNewTrainingProviderViewModelValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new EnterNewTrainingProviderViewModelValidator();
        }

        [Test, MoqAutoData]
        public void WhenValidatingNewTrainingProvider_AndAnUkprnIsNotGiven_ThenValidatorReturnsInvalid(EnterNewTrainingProviderViewModel viewModel)
        {
            viewModel.Ukprn = 0;
            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
            Assert.AreEqual(ExpectedUkprnErrorMessage, result.Errors.First().ErrorMessage);
        }

        [Test, MoqAutoData]
        public void WhenValidatingNewTrainingProvider_AndAnUkprnIsGiven_ThenValidatorReturnsValid(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }

        [Test, MoqAutoData]
        public void When_ValidatingNewTrainingProvider_And_UkprnIsTheSameAsCurrentUkprn_Then_ValidatorReturnsInvalid(EnterNewTrainingProviderViewModel viewModel)
        {
            viewModel.Ukprn = 100;
            viewModel.CurrentProviderId = 100;

            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }
    }
}
