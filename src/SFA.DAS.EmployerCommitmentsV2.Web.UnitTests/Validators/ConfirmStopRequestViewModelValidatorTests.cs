using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class ConfirmStopRequestViewModelValidatorTests
    {
        private ConfirmStopRequestViewModelValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new ConfirmStopRequestViewModelValidator();
        }

        [Test, MoqAutoData]
        public void WhenStopConfirmedIsNull_ThenValidatorReturnsInvalid(ConfirmStopRequestViewModel viewModel)
        {
            viewModel.StopConfirmed = null;

            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenStopConfirmedIsFalse_ThenValidatorReturnsValid(ConfirmStopRequestViewModel viewModel)
        {
            viewModel.StopConfirmed = false;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenStopConfirmedIsTrue_ThenValidatorReturnsValid(ConfirmStopRequestViewModel viewModel)
        {
            viewModel.StopConfirmed = true;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }
    }
}
