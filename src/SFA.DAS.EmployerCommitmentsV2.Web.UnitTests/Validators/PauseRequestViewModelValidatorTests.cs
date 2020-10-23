using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class PauseRequestViewModelValidatorTests
    {
        private PauseRequestViewModelValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new PauseRequestViewModelValidator();
        }

        [Test, MoqAutoData]
        public void WhenPauseConfirmedIsNull_ThenValidatorReturnsInvalid(PauseRequestViewModel viewModel)
        {
            viewModel.PauseConfirmed = null;

            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenPauseConfirmedIsFalse_ThenValidatorReturnsValid(PauseRequestViewModel viewModel)
        {
            viewModel.PauseConfirmed = false;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenPauseConfirmedIsTrue_ThenValidatorReturnsValid(PauseRequestViewModel viewModel)
        {
            viewModel.PauseConfirmed = true;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }
    }
}
