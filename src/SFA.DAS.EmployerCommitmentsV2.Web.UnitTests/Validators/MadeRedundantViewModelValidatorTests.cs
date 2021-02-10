using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class MadeRedundantViewModelValidatorTests
    {
        private MadeRedundantViewModelValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new MadeRedundantViewModelValidator();
        }

        [Test, MoqAutoData]
        public void WhenMadeRedundantConfirmedIsNull_ThenValidatorReturnsInvalid(MadeRedundantViewModel viewModel)
        {
            viewModel.MadeRedundant = null;

            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenMadeRedundantConfirmedIsFalse_ThenValidatorReturnsValid(MadeRedundantViewModel viewModel)
        {
            viewModel.MadeRedundant = false;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenMadeRedundantConfirmedIsTrue_ThenValidatorReturnsValid(MadeRedundantViewModel viewModel)
        {
            viewModel.MadeRedundant = true;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }
    }
}
