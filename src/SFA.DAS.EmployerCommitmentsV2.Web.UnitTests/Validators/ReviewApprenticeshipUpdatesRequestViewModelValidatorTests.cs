using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class ReviewApprenticeshipUpdatesRequestViewModelValidatorTests
    {
        private ReviewApprenticeshipUpdatesRequestViewModelValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new ReviewApprenticeshipUpdatesRequestViewModelValidator();
        }

        [Test, MoqAutoData]
        public void WhenApproveChangesIsNull_ThenValidatorReturnsInvalid(ReviewApprenticeshipUpdatesRequestViewModel viewModel)
        {
            viewModel.ApproveChanges = null;

            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenApproveChangesIsFalse_ThenValidatorReturnsValid(ReviewApprenticeshipUpdatesRequestViewModel viewModel)
        {
            viewModel.ApproveChanges = false;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }

        [Test, MoqAutoData]
        public void WhenApproveChangesIsTrue_ThenValidatorReturnsValid(ReviewApprenticeshipUpdatesRequestViewModel viewModel)
        {
            viewModel.ApproveChanges = true;

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }
    }
}
