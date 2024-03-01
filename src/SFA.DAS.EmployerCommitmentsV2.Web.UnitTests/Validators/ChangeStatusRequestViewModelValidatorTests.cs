using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture()]
public class ChangeStatusRequestViewModelValidatorTests : ValidatorTestBase<ChangeStatusRequestViewModel, ChangeStatusRequestViewModelValidator>
{
    [TestCase(ChangeStatusType.Stop)]
    [TestCase(ChangeStatusType.Pause)]
    [TestCase(ChangeStatusType.GoBack)]
    public void Validate_ChangeStatusType_ShouldBeValidated(ChangeStatusType status)
    {
        var model = new ChangeStatusRequestViewModel { SelectedStatusChange = status };

        AssertValidationResult(request => request.SelectedStatusChange, model, true);
    }

    [Test]
    public void Validate_ChangeStatusType_WhenNotSelected_ThenItShouldBeInvalid()
    {
        var model = new ChangeStatusRequestViewModel { SelectedStatusChange = null };

        AssertValidationResult(request => request.SelectedStatusChange, model, false);
    }
}