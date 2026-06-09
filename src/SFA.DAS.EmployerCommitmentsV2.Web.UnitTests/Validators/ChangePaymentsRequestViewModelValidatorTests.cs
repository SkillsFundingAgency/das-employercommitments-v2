using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ChangePaymentsRequestViewModelValidatorTests : ValidatorTestBase<ChangePaymentsRequestViewModel, ChangePaymentsRequestViewModelValidator>
{
    [Test]
    public void Validate_WhenPauseAndChangeNotSelected_ThenInvalid()
    {
        var model = new ChangePaymentsRequestViewModel { FreezeStatus = false, ChangeConfirmed = null };

        AssertValidationResult(request => request.ChangeConfirmed, model, false, "Select if you would like to pause payments");
    }

    [Test]
    public void Validate_WhenResumeAndChangeNotSelected_ThenInvalid()
    {
        var model = new ChangePaymentsRequestViewModel { FreezeStatus = true, ChangeConfirmed = null };

        AssertValidationResult(request => request.ChangeConfirmed, model, false, "Select if you would like to resume payments");
    }

    [Test]
    public void Validate_WhenPauseConfirmedWithoutReason_ThenInvalid()
    {
        var model = new ChangePaymentsRequestViewModel
        {
            FreezeStatus = false,
            ChangeConfirmed = true,
            FreezePaymentsReason = null
        };

        AssertValidationResult(request => request.FreezePaymentsReason, model, false, "Select a reason for pausing payments");
    }

    [Test]
    public void Validate_WhenPauseWithoutConfirmationOrReason_ThenInvalidForReason()
    {
        var model = new ChangePaymentsRequestViewModel
        {
            FreezeStatus = false,
            ChangeConfirmed = null,
            FreezePaymentsReason = null
        };

        AssertValidationResult(request => request.FreezePaymentsReason, model, false, "Select a reason for pausing payments");
    }

    [Test]
    public void Validate_WhenPauseDeclined_ThenValidWithoutReason()
    {
        var model = new ChangePaymentsRequestViewModel
        {
            FreezeStatus = false,
            ChangeConfirmed = false,
            FreezePaymentsReason = null
        };

        AssertValidationResult(request => request.FreezePaymentsReason, model, true);
    }

    [Test]
    public void Validate_WhenPauseConfirmedWithReason_ThenValid()
    {
        var model = new ChangePaymentsRequestViewModel
        {
            FreezeStatus = false,
            ChangeConfirmed = true,
            FreezePaymentsReason = 1
        };

        AssertValidationResult(request => request.FreezePaymentsReason, model, true);
    }

    [Test]
    public void Validate_WhenResumeConfirmed_ThenValidWithoutReason()
    {
        var model = new ChangePaymentsRequestViewModel
        {
            FreezeStatus = true,
            ChangeConfirmed = true,
            FreezePaymentsReason = null
        };

        AssertValidationResult(request => request.ChangeConfirmed, model, true);
    }
}
