using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticeshipDetailsRequestViewModel : IAuthorizationContextModel
    {
        public string HashedApprenticeshipId { get; set; }
        public string AccountHashedId { get; set; }
        public string ApprenticeName { get; set; }
        public string ULN { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StopDate { get; set; }
        public DateTime? PauseDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public ProgrammeType? TrainingType { get; set; }
        public string TrainingName { get; set; }
        public decimal? Cost { get; set; }
        public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
        public string Status { get; set; }
        public string ProviderName { get; set; }
        public PendingChanges PendingChanges { get; set; }
        public bool CanEditStatus { get; set; }
        public string EmployerReference { get; set; }
        public string CohortReference { get; set; }
        public bool EnableEdit { get; set; }
        public bool PendingDataLockRestart { get; set; }
        public bool PendingDataLockChange { get; set; }
        public string EndpointAssessorName { get; set; }
        public bool? MadeRedundant { get; set; }
        public bool HasPendingChangeOfProviderRequest { get; set; }
        public Party? PendingChangeOfProviderRequestWithParty { get; set; }
        public string HashedNewApprenticeshipId { get; set; }
        public bool IsContinuation { get; set; }
        public string HashedPreviousApprenticeshipId { get; set; }
        public bool ShowChangeTrainingProviderLink => ((ApprenticeshipStatus == ApprenticeshipStatus.Stopped ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.Paused ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.Live ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.WaitingToStart) &&
                                                       !HasPendingChangeOfProviderRequest &&
                                                       !HasPendingChangeOfEmployerRequest &&
                                                       !(HasApprovedChangeOfEmployerRequest && !IsContinuation) &&
                                                       string.IsNullOrEmpty(HashedNewApprenticeshipId));

        public bool HasApprovedChangeOfProviderRequest { get; set; }
        public bool HasPendingChangeOfEmployerRequest { get; set; }
        public Party? PendingChangeOfEmployerRequestWithParty { get; set; }
        public bool HasApprovedChangeOfEmployerRequest { get; set; }
        public bool IsV2Edit { get; set; }

        public ActionRequiredBanner GetActionRequiredBanners()
        {
            var actionRequiredBanner = ActionRequiredBanner.None;

            actionRequiredBanner |= PendingChanges == PendingChanges.ReadyForApproval
                ? ActionRequiredBanner.PendingChangeForApproval
                : actionRequiredBanner;

            actionRequiredBanner |= HasPendingChangeOfProviderRequest &&
                    PendingChangeOfProviderRequestWithParty.HasValue &&
                    PendingChangeOfProviderRequestWithParty.Value == Party.Employer
                ? ActionRequiredBanner.InFlightChangeOfProviderPendingEmployer
                : actionRequiredBanner;

            actionRequiredBanner |= PendingDataLockChange
                ? ActionRequiredBanner.DataLockChange
                : actionRequiredBanner;

            actionRequiredBanner |= PendingDataLockRestart
                ? ActionRequiredBanner.DataLockRestart
                : actionRequiredBanner;

            return actionRequiredBanner;
        }

        public ChangeToApprenticeshipBanner GetChangeToApprenticeshipBanners()
        {
            var changeToApprenticeshipBanner = ChangeToApprenticeshipBanner.None;

            changeToApprenticeshipBanner |= PendingChanges == PendingChanges.WaitingForApproval
                ? ChangeToApprenticeshipBanner.PendingChangeWaitingForApproval
                : changeToApprenticeshipBanner;

            changeToApprenticeshipBanner |= HasPendingChangeOfProviderRequest && PendingChangeOfProviderRequestWithParty.HasValue
               && PendingChangeOfProviderRequestWithParty.Value != Party.Employer
                ? ChangeToApprenticeshipBanner.InFlightChangeOfProviderPendingOther
                : changeToApprenticeshipBanner;

            return changeToApprenticeshipBanner;
        }
    }

    public enum PendingChanges
    {
        None = 0,
        ReadyForApproval = 1,
        WaitingForApproval = 2
    }

    [Flags]
    public enum ActionRequiredBanner
    {
        None = 0,
        PendingChangeForApproval = 1,
        InFlightChangeOfProviderPendingEmployer = 2,
        DataLockChange = 4,
        DataLockRestart = 8
    }

    [Flags]
    public enum ChangeToApprenticeshipBanner
    {
        None = 0,
        PendingChangeWaitingForApproval = 1,
        InFlightChangeOfProviderPendingOther = 2
    }
}
