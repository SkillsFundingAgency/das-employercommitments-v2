using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ConfirmDeleteViewModelValidatorTests : ValidatorTestBase<ConfirmDeleteViewModel, ConfirmDeleteViewModelValidator>
{
    [TestCase(null, false)]
    [TestCase(true, true)]
    [TestCase(false, true)]
    public void Validate_ConfirmDeletion_ShouldBeValidated(bool? confirmDeletion, bool expectedValid)
    {
        var model = new ConfirmDeleteViewModel() { ConfirmDeletion = confirmDeletion };
        AssertValidationResult(request => request.ConfirmDeletion, model, expectedValid);
    }
}