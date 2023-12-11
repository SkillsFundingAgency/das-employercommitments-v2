using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ReviewApprenticeshipUpdatesViewModelValidatorTests : ValidatorTestBase<ReviewApprenticeshipUpdatesViewModel, ReviewApprenticeshipUpdatesViewModelValidator>
{
    [Test, MoqAutoData]
    public void WhenApproveChangesIsNull_ThenValidatorReturnsInvalid(ReviewApprenticeshipUpdatesViewModel viewModel)
    {
        viewModel.ApproveChanges = null;

        AssertValidationResult(x => x.ApproveChanges, viewModel, false);
    }

    [Test, MoqAutoData]
    public void WhenApproveChangesIsFalse_ThenValidatorReturnsValid(ReviewApprenticeshipUpdatesViewModel viewModel)
    {
        viewModel.ApproveChanges = false;

        AssertValidationResult(x => x.ApproveChanges, viewModel, true);
    }

    [Test, MoqAutoData]
    public void WhenApproveChangesIsTrue_ThenValidatorReturnsValid(ReviewApprenticeshipUpdatesViewModel viewModel)
    {
        viewModel.ApproveChanges = true;

        AssertValidationResult(x => x.ApproveChanges, viewModel, true);
    }
}