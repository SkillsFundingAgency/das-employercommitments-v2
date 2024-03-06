using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;

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
        public DateTime? ActualStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StopDate { get; set; }
        public DateTime? PauseDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public ProgrammeType? TrainingType { get; set; }
        public string TrainingName { get; set; }
        public string Version { get; set; }
        public string Option { get; set; }
        public IEnumerable<string> VersionOptions { get; set; }
        public decimal? Cost { get; set; }
        public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
        public string ProviderName { get; set; }
        public PendingChanges PendingChanges { get; set; }

        public bool CanEditStatus => (ApprenticeshipStatus == ApprenticeshipStatus.Live ||
                                     ApprenticeshipStatus == ApprenticeshipStatus.WaitingToStart ||
                                     ApprenticeshipStatus == ApprenticeshipStatus.Paused);

        public string EmployerReference { get; set; }
        public string CohortReference { get; set; }
        public bool EnableEdit { get; set; }
        public bool PendingDataLockRestart { get; set; }
        public bool PendingDataLockChange { get; set; }
        public string EndpointAssessorName { get; set; }
        public bool? MadeRedundant { get; set; }
        public bool HasPendingChangeOfProviderRequest { get; set; }
        public Party? PendingChangeOfProviderRequestWithParty { get; set; }
        public bool HasContinuation { get; set; }

        public bool ShowChangeTrainingProviderLink => ((ApprenticeshipStatus == ApprenticeshipStatus.Stopped ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.Paused ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.Live ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.WaitingToStart) &&
                                                       !HasContinuation &&
                                                       DeliveryModel != CommitmentsV2.Types.DeliveryModel.PortableFlexiJob
                                                       );

        public List<TrainingProviderHistory> TrainingProviderHistory { get; set; }
        public ConfirmationStatus? ConfirmationStatus { get; set; }
        public string Email { get; set; }
        public bool EmailShouldBePresent { get; set; }
        public bool HasNewerVersions { get; set; }

        // If It's completed or stopped and option is null, dont show options as it could predate standard versioning
        // even if the version has options
        public bool ShowOptions => VersionOptions != null && VersionOptions.Any() && !PreDatesStandardVersioning;

        public bool OnlySingleOption => VersionOptions != null && VersionOptions.Count() == 1;
        private bool IsCompletedOrStopped => ApprenticeshipStatus == ApprenticeshipStatus.Stopped || ApprenticeshipStatus == ApprenticeshipStatus.Completed;
        private bool PreDatesStandardVersioning => IsCompletedOrStopped && Option == null;
        public bool EmailAddressConfirmedByApprentice { get; set; }
        public bool CanResendInvitation => !string.IsNullOrEmpty(Email) && !EmailAddressConfirmedByApprentice && ApprenticeshipStatus != ApprenticeshipStatus.Stopped;

        public DeliveryModel? DeliveryModel { get; set; }
        public int? EmploymentPrice { get; set; }
        public string EmploymentPriceDisplay => EmploymentPrice?.ToGdsCostFormat() ?? string.Empty;
        public DateTime? EmploymentEndDate { get; set; }
        public string EmploymentEndDateDisplay => EmploymentEndDate?.ToGdsFormatWithoutDay() ?? string.Empty;
        public bool? RecognisePriorLearning { get; set; }
        public int? DurationReducedBy { get; set; }
        public int? PriceReducedBy { get; set; }
        public int? TrainingTotalHours { get; set; }
        public int? DurationReducedByHours { get; set; }
        public bool? IsDurationReducedByRpl { get; set; }

        public bool HasPendingOverlappingTrainingDateRequest { get; set; }

        public bool HasMultipleDeliveryModelOptions { get; set; }
        public bool? IsOnFlexiPaymentPilot { get; set; }

        public PendingPriceChange PendingPriceChange { get; set; }
        public bool HasPendingPriceChange => PendingPriceChange != null;
        public bool HasPendingProviderInitiatedPriceChange => PendingPriceChange?.ProviderApprovedDate != null;
        public string PriceChangeUrl { get; set; }
        public string PendingPriceChangeUrl { get; set; }
        public bool ShowPriceChangeRejected { get; set; }
        public bool ShowPriceChangeApproved { get; set; }
        public bool ShowPriceChangeRequestSent { get; set; }

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

            actionRequiredBanner |= HasPendingOverlappingTrainingDateRequest
              ? ActionRequiredBanner.PendingOverlappingTrainingDateRequest
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
        DataLockRestart = 8,
        PendingOverlappingTrainingDateRequest = 16
    }

    [Flags]
    public enum ChangeToApprenticeshipBanner
    {
        None = 0,
        PendingChangeWaitingForApproval = 1,
        InFlightChangeOfProviderPendingOther = 2
    }

    public class TrainingProviderHistory
    {
        public string ProviderName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string HashedApprenticeshipId { get; set; }
        public bool ShowLink { get; set; }
    }

    public class PendingPriceChange
    {
        public decimal Cost { get; set; }
        public decimal? TrainingPrice { get; set; }
        public decimal? EndPointAssessmentPrice { get; set; }
        public DateTime? ProviderApprovedDate { get; set; }
        public DateTime? EmployerApprovedDate { get; set; }
    }

    public enum InitiatedBy
    {
        Provider,
        Employer
    }

    public static class PendingPriceChangeExtensions
    {
        public static InitiatedBy GetPriceChangeInitiatedBy(this PendingPriceChange pendingPriceChange)
        {
            if (pendingPriceChange.ProviderApprovedDate.HasValue && !pendingPriceChange.EmployerApprovedDate.HasValue)
            {
                return InitiatedBy.Provider;
            }

            if (!pendingPriceChange.ProviderApprovedDate.HasValue && pendingPriceChange.EmployerApprovedDate.HasValue)
            {
                return InitiatedBy.Employer;
            }

            throw new ArgumentOutOfRangeException("Could not resolve PriceChange Initiator, expected at least one approval date to be populated");
        }
    }
}