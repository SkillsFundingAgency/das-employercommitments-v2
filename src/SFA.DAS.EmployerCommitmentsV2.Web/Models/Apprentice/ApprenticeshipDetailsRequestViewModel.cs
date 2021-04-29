using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Collections.Generic;

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
        public bool ShowChangeTrainingProviderLink => ((ApprenticeshipStatus == ApprenticeshipStatus.Stopped ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.Paused ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.Live ||
                                                       ApprenticeshipStatus == ApprenticeshipStatus.WaitingToStart) &&
                                                       !HasPendingChangeOfProviderRequest &&
                                                       !HasPendingChangeOfEmployerRequest &&
                                                       string.IsNullOrEmpty(HashedNewApprenticeshipId));

        public bool HasPendingChangeOfEmployerRequest { get; set; }
        public Party? PendingChangeOfEmployerRequestWithParty { get; set; }
        public List<TrainingProviderHistory> TrainingProviderHistory { get; set; }
        public bool IsV2Edit { get; set; }
    }

    public enum PendingChanges
    {
        None = 0,
        ReadyForApproval = 1,
        WaitingForApproval = 2
    }

    public class TrainingProviderHistory
    {
        public string ProviderName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string HashedApprenticeshipId { get; set; }
        public bool ShowLink { get; set; }
    }
}
