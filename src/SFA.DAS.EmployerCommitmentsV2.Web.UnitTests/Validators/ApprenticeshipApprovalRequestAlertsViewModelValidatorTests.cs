using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class ApprenticeshipApprovalRequestAlertsViewModelValidatorTests : ValidatorTestBase<ApprenticeshipApprovalRequestAlertsViewModel, ApprenticeshipApprovalRequestAlertsViewModelValidator>
{
    [Test]
    public void ThenValidatesAlertAcknowledged()
    {
        var request = CreateRequests(true, false, false);

        AssertValidationResult(x => x.ApprovalRequests, request, true);
    }

    [Test]
    public void ThenValidatesAlertAcknowledged_IfAnyALertRequestIsNotAcknowledged()
    {
        var request = CreateRequests(true, false, false, null);

        AssertValidationResult(x => x.ApprovalRequests, request, false, "Select if you would like to delete this alert");
    }

    [Test]
    public void ThenValidatesAlertAcknowledged_IfAllALertRequestIsNotAcknowledged()
    {
        var request = CreateRequests(null, null, null, null);

        AssertValidationResult(x => x.ApprovalRequests, request, false, "Select if you would like to delete this alert");
    }

    public ApprenticeshipApprovalRequestAlertsViewModel CreateRequests(params bool?[] alertAcknowledged)
    {
        return new ApprenticeshipApprovalRequestAlertsViewModel
        {
            ApprovalRequests = alertAcknowledged.Select((a, i) => new ApprovalRequestAlertViewModel()
            {
                Id = Guid.CreateVersion7(),
                Seen = alertAcknowledged[i]
            }).ToList()
        };
    }
}