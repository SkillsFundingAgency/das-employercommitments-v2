using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models;

public class ApprenticeshipDetailsRequestViewModelTests
{
    [TestCaseSource(nameof(ActionRequiredBannerCases))]
    public void ThenTheExpectedActionRequiredBannersShouldBeShown(
        PendingChanges pendingChanges,
        bool hasPendingChangeOfProviderRequest,
        Party pendingChangeOfProviderRequestWithParty,
        bool pendingDataLockChange,
        bool pendingDataLockRestart,
        bool pendingOverlappingTrainingDateRequest,
        ActionRequiredBanner expected)
    {
        //Arrange
        var viewModel = new ApprenticeshipDetailsRequestViewModel
        {
            PendingChanges = pendingChanges,
            HasPendingChangeOfProviderRequest = hasPendingChangeOfProviderRequest,
            PendingChangeOfProviderRequestWithParty = pendingChangeOfProviderRequestWithParty,
            PendingDataLockChange = pendingDataLockChange,
            PendingDataLockRestart = pendingDataLockRestart,
            HasPendingOverlappingTrainingDateRequest = pendingOverlappingTrainingDateRequest
        };

        //Act
        var actionBanners = viewModel.GetActionRequiredBanners();

        //Assert
        Assert.That(actionBanners.HasFlag(expected), Is.True);
    }

    private static object[] ActionRequiredBannerCases =
    {
        new object[] { PendingChanges.None, false, Party.None, false, false, false, ActionRequiredBanner.None },
        new object[] { PendingChanges.ReadyForApproval, false, Party.None, false, false, false, ActionRequiredBanner.PendingChangeForApproval },
        new object[] { PendingChanges.None, true, Party.Employer, false, false, false, ActionRequiredBanner.InFlightChangeOfProviderPendingEmployer },
        new object[] { PendingChanges.None, false, Party.None, true, false, false, ActionRequiredBanner.DataLockChange },
        new object[] { PendingChanges.None, false, Party.None, false, true, false, ActionRequiredBanner.DataLockRestart },
        new object[] { PendingChanges.ReadyForApproval, true, Party.Employer, false, false, false, ActionRequiredBanner.PendingChangeForApproval | ActionRequiredBanner.InFlightChangeOfProviderPendingEmployer },
        new object[] { PendingChanges.None, false, Party.None, true, true, false, ActionRequiredBanner.DataLockChange | ActionRequiredBanner.DataLockRestart },
        new object[] { PendingChanges.ReadyForApproval, true, Party.Employer, true, true, false, ActionRequiredBanner.PendingChangeForApproval | ActionRequiredBanner.InFlightChangeOfProviderPendingEmployer | ActionRequiredBanner.DataLockChange | ActionRequiredBanner.DataLockRestart },
        new object[] { PendingChanges.None, false, Party.None, false, false, true, ActionRequiredBanner.PendingOverlappingTrainingDateRequest }
    };

    [TestCaseSource(nameof(ChangeToApprenticeshipBannerCases))]
    public void ThenTheExpectedChangeToApprenticeshipBannersShouldBeShown(
        PendingChanges pendingChanges,
        bool hasPendingChangeOfProviderRequest,
        Party pendingChangeOfProviderRequestWithParty,
        ChangeToApprenticeshipBanner expected)
    {
        //Arrange
        var viewModel = new ApprenticeshipDetailsRequestViewModel
        {
            PendingChanges = pendingChanges,
            HasPendingChangeOfProviderRequest = hasPendingChangeOfProviderRequest,
            PendingChangeOfProviderRequestWithParty = pendingChangeOfProviderRequestWithParty
        };

        //Act
        var changeToApprenticeshipBanners = viewModel.GetChangeToApprenticeshipBanners();

        //Assert
        Assert.That(changeToApprenticeshipBanners.HasFlag(expected), Is.True);
    }

    private static object[] ChangeToApprenticeshipBannerCases =
    {
        new object[] { PendingChanges.None, false, Party.None, ChangeToApprenticeshipBanner.None },
        new object[] { PendingChanges.WaitingForApproval, false, Party.None, ChangeToApprenticeshipBanner.PendingChangeWaitingForApproval },
        new object[] { PendingChanges.None, true, Party.Provider, ChangeToApprenticeshipBanner.InFlightChangeOfProviderPendingOther },
        new object[] { PendingChanges.WaitingForApproval, true, Party.Provider, ChangeToApprenticeshipBanner.PendingChangeWaitingForApproval | ChangeToApprenticeshipBanner.InFlightChangeOfProviderPendingOther }
    };
}