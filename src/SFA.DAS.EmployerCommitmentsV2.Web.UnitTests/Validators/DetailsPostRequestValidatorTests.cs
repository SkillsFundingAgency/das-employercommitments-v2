using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class DetailsViewModelValidatorTests : ValidatorTestBase<DetailsViewModel, DetailsViewModelValidator>
    {
        [TestCase(null, false)]
        [TestCase(CohortDetailsOptions.Send, true)]
        [TestCase(CohortDetailsOptions.Approve, true)]
        public void Validate_Selection_ShouldBeValidated(CohortDetailsOptions? selection, bool expectedValid)
        {
            var model = new DetailsViewModel { Selection = selection };
            AssertValidationResult(request => request.Selection, model, expectedValid);
        }
    }
}
