using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class ViewApprenticeshipUpdatesViewModelValidatorTests
    {
        private ViewApprenticeshipUpdatesViewModelValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new ViewApprenticeshipUpdatesViewModelValidator();
        }

        [Test, MoqAutoData]
        public void WhenUndoChangesIsNull_ThenValidatorReturnsInvalid(ViewApprenticeshipUpdatesViewModel viewModel)
        {
            viewModel.UndoChanges = null;

            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenUndoChangesIsFalse_ThenValidatorReturnsValid(ViewApprenticeshipUpdatesViewModel viewModel)
        {
            viewModel.UndoChanges = false;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenUndoChangesIsTrue_ThenValidatorReturnsValid(ViewApprenticeshipUpdatesViewModel viewModel)
        {
            viewModel.UndoChanges = true;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }
    }
}
