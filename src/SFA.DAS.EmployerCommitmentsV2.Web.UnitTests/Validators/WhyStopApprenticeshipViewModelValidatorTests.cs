using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.WhyStopApprenticeshipViewModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class WhyStopApprenticeshipViewModelValidatorTests : ValidatorTestBase<WhyStopApprenticeshipViewModel,WhyStopApprenticeshipViewModelValidator>
{
    [TestCase(StopStatusReason.ChangeProvider)]
    [TestCase(StopStatusReason.NeverStarted)]
    [TestCase(StopStatusReason.Withdrawn)]
    [TestCase(StopStatusReason.ProviderCorrectsApprenticeRecord)]
    [TestCase(StopStatusReason.LeftEmployment)]
    public void Validate_StopStatusReason_ShouldBeValidated(StopStatusReason status)
    {
        var model = new WhyStopApprenticeshipViewModel { SelectedStatusChange = status };

        AssertValidationResult(request => request.SelectedStatusChange, model, true);
    }

    [Test]
    public void Validate_ChangeStatusType_WhenNotSelected_ThenItShouldBeInvalid()
    {
        var model = new WhyStopApprenticeshipViewModel { SelectedStatusChange = null };

        AssertValidationResult(request => request.SelectedStatusChange, model, false);
    }
}