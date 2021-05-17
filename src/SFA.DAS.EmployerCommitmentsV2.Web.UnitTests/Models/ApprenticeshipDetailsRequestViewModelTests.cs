using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models
{
    public class ApprenticeshipDetailsRequestViewModelTests
    {
        [TestCaseSource(nameof(ActionRequiredBannerCases))]
        public void ThenTheExpectedActionRequiredBannersShouldBeShown(
            PendingChanges pendingChanges, 
            bool hasPendingChangeOfProviderRequest,
            Party pendingChangeOfProviderRequestWithParty,
            bool pendingDataLockChange,
            bool pendingDataLockRestart,
            ActionRequiredBanner expected)
        {
            //Arrange
            var viewModel = new ApprenticeshipDetailsRequestViewModel
            {
                PendingChanges = pendingChanges,
                HasPendingChangeOfProviderRequest = hasPendingChangeOfProviderRequest,
                PendingChangeOfProviderRequestWithParty = pendingChangeOfProviderRequestWithParty,
                PendingDataLockChange = pendingDataLockChange,
                PendingDataLockRestart = pendingDataLockRestart
            };

            //Act
            var actionBanners = viewModel.GetActionRequiredBanners();

            //Assert
            Assert.IsTrue(actionBanners.HasFlag(expected));
        }

        static object[] ActionRequiredBannerCases =
        {
            new object[] { PendingChanges.None, false, Party.None, false, false, ActionRequiredBanner.None },
            new object[] { PendingChanges.ReadyForApproval, false, Party.None, false, false, ActionRequiredBanner.PendingChangeForApproval },
            new object[] { PendingChanges.None, true, Party.Employer, false, false, ActionRequiredBanner.InFlightChangeOfProviderPendingEmployer },
            new object[] { PendingChanges.None, false, Party.None, true, false, ActionRequiredBanner.DataLockChange },
            new object[] { PendingChanges.None, false, Party.None, false, true, ActionRequiredBanner.DataLockRestart },
            new object[] { PendingChanges.ReadyForApproval, true, Party.Employer, false, false, ActionRequiredBanner.PendingChangeForApproval | ActionRequiredBanner.InFlightChangeOfProviderPendingEmployer },
            new object[] { PendingChanges.None, false, Party.None, true, true, ActionRequiredBanner.DataLockChange | ActionRequiredBanner.DataLockRestart },
            new object[] { PendingChanges.ReadyForApproval, true, Party.Employer, true, true, ActionRequiredBanner.PendingChangeForApproval | ActionRequiredBanner.InFlightChangeOfProviderPendingEmployer | ActionRequiredBanner.DataLockChange | ActionRequiredBanner.DataLockRestart }
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
            Assert.IsTrue(changeToApprenticeshipBanners.HasFlag(expected));
        }

        static object[] ChangeToApprenticeshipBannerCases =
        {
            new object[] { PendingChanges.None, false, Party.None, ChangeToApprenticeshipBanner.None },
            new object[] { PendingChanges.WaitingForApproval, false, Party.None, ChangeToApprenticeshipBanner.PendingChangeWaitingForApproval },
            new object[] { PendingChanges.None, true, Party.Provider, ChangeToApprenticeshipBanner.InFlightChangeOfProviderPendingOther },
            new object[] { PendingChanges.WaitingForApproval, true, Party.Provider, ChangeToApprenticeshipBanner.PendingChangeWaitingForApproval | ChangeToApprenticeshipBanner.InFlightChangeOfProviderPendingOther }
        };
    }
}