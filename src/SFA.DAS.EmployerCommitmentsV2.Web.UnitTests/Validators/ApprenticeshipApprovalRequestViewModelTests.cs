using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ApprenticeshipApprovalRequestViewModelTests : ValidatorTestBase<ApprenticeshipApprovalRequestViewModel, ApprenticeshipApprovalRequestViewModelValidator>
{
    [TestCase(null, false)]
    [TestCase(true, true)]
    [TestCase(false, true)]
    public void Validate_FinishDate_ShouldBeValidated(bool? apply, bool expectedValid)
    {
        var model = new ApprenticeshipApprovalRequestViewModel { ApproveChanges = apply };
        AssertValidationResult(request => request.ApproveChanges, model, expectedValid);
    }
}