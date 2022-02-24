using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class ViewApprenticeshipUpdatesViewModelValidatorTests : ValidatorTestBase<ViewApprenticeshipUpdatesViewModel, ViewApprenticeshipUpdatesViewModelValidator>
    {
        [Test, MoqAutoData]
        public void WhenUndoChangesIsNull_ThenValidatorReturnsInvalid(ViewApprenticeshipUpdatesViewModel viewModel)
        {
            viewModel.UndoChanges = null;

            AssertValidationResult(x => x.UndoChanges, viewModel, false);
        }

        [Test, MoqAutoData]
        public void WhenUndoChangesIsFalse_ThenValidatorReturnsValid(ViewApprenticeshipUpdatesViewModel viewModel)
        {
            viewModel.UndoChanges = false;

            AssertValidationResult(x => x.UndoChanges, viewModel, true);
        }

        [Test, MoqAutoData]
        public void WhenUndoChangesIsTrue_ThenValidatorReturnsValid(ViewApprenticeshipUpdatesViewModel viewModel)
        {
            viewModel.UndoChanges = true;

            AssertValidationResult(x => x.UndoChanges, viewModel, true);
        }
    }
}
