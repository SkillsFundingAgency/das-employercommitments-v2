using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class SelectOptionViewModelValidatorTests
    {
        private SelectOptionViewModelValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new SelectOptionViewModelValidator();
        }

        [Test, MoqAutoData]
        public void WhenNoOptionIsSelected_ThenValidatorReturnsInvalid(SelectOptionViewModel viewModel)
        {
            viewModel.CourseOption = null;

            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }
    }
}
