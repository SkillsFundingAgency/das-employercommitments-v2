using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class SelectOptionViewModelValidatorTests : ValidatorTestBase<SelectOptionViewModel, SelectOptionViewModelValidator>
    {
        [Test]
        public void WhenNoOptionIsSelected_ThenValidatorReturnsInvalid()
        {

            var viewModel = new SelectOptionViewModel { CourseOption = null };

            AssertValidationResult(x => x.CourseOption, viewModel, false);
        }
    }
}
