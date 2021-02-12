using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticeshipDetailsRequestViewModel
    {
        public string HashedApprenticeshipId { get; set; }

        public string AccountHashedId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

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

        //public PaymentStatus PaymentStatus { get; set; }
        public ApprenticeshipStatus PaymentStatus { get; set; }

        public string Status { get; set; }

        public string ProviderName { get; set; }

        public PendingChanges PendingChanges { get; set; }

        public bool CanEditStatus { get; set; }

        public IEnumerable<string> Alerts { get; set; }

        public string EmployerReference { get; set; }

        public string CohortReference { get; set; }

        public bool EnableEdit { get; set; }

        public bool PendingDataLockRestart { get; set; }

        public bool PendingDataLockChange { get; set; }

        public bool CanEditStopDate { get; set; }

        public string EndpointAssessorName { get; set; }

        public Guid? ReservationId { get; set; }
        public string ManageApprenticeshipV2PageLink { get; set; }

        public string EditApprenticeshipEndDateLink { get; set; }

        public bool? MadeRedundant { get; set; }
        public string ChangeProviderLink { get; set; }


        public bool HasPendingChangeOfProviderRequest { get; set; }
        public Party? PendingChangeOfProviderRequestWithParty { get; set; }


        public bool HasApprovedChangeOfPartyRequest { get; set; }
        public string HashedNewApprenticeshipId { get; set; }
        public bool IsContinuation { get; set; }
        public bool IsChangeOfProviderContinuation { get; set; }
        public string HashedPreviousApprenticeshipId { get; set; }

        public string ViewChangesLink { get; internal set; }

        public bool ShowChangeTrainingProviderLink => (PaymentStatus == ApprenticeshipStatus.Stopped &&
                                                      !HasPendingChangeOfProviderRequest &&
                                                      !HasPendingChangeOfEmployerRequest &&
                                                      !(HasApprovedChangeOfEmployerRequest && !IsContinuation) &&
                                                      !string.IsNullOrEmpty(ChangeProviderLink) &&
                                                      string.IsNullOrEmpty(HashedNewApprenticeshipId));

        public bool HasApprovedChangeOfProviderRequest { get; set; }
        public bool HasPendingChangeOfEmployerRequest { get; set; }
        public Party? PendingChangeOfEmployerRequestWithParty { get; set; }
        public bool HasApprovedChangeOfEmployerRequest { get; set; }
    }
    public enum PendingChanges
    {
        None = 0,
        ReadyForApproval = 1,
        WaitingForApproval = 2
    }

    public enum TriageStatusViewModel
    {
        Unknown = 0,
        Change = 1,
        Restart = 2,
        FixIlr = 3
    }

    //TODO : do we need to move this to commitment v2 api type
    public enum TrainingType
    {
        Standard = 0,
        Framework = 1
    }
}
