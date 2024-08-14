using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class SelectCourseViewModelValidatorTests : ValidatorTestBase<SelectCourseViewModel, SelectCourseViewModelValidator>
    {
        private const string _errorMessage = "Select an apprenticeship course";

        [Test, MoqAutoData]
        public void And_CourseCode_IsNull_ThenReturnsInvalid(SelectCourseViewModel viewModel)
        {
            viewModel.CourseCode = null;

            AssertValidationResult(x => x.CourseCode, viewModel, false, _errorMessage);
        }

        [Test, MoqAutoData]
        public void And_CourseCode_IsEmpty_ThenReturnsInvalid(SelectCourseViewModel viewModel)
        {
            viewModel.CourseCode = string.Empty;

            AssertValidationResult(x => x.CourseCode, viewModel, false, _errorMessage);
        }

        [Test, MoqAutoData]
        public void And_CourseCode_IsValid_ThenReturns_Valid(SelectCourseViewModel viewModel)
        {
            viewModel.CourseCode = "123";

            AssertValidationResult(x => x.CourseCode, viewModel, true);
        }
    }
}
