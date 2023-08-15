using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses
{
    public class GetManageApprenticeshipDetailsResponse
    {
        public GetApprenticeshipResponse Apprenticeship { get; set; }
        public IEnumerable<GetPriceEpisodeResponse.PriceEpisode> PriceEpisodes { get; set; }
        public IEnumerable<GetApprenticeshipUpdateResponse.ApprenticeshipUpdate> ApprenticeshipUpdates { get; set; }
        public IReadOnlyCollection<GetDataLockResponse.DataLock> DataLocks { get; set; }
        public IReadOnlyCollection<GetChangeOfPartyRequestResponse.ChangeOfPartyRequest> ChangeOfPartyRequests { get; set; }
        public IReadOnlyCollection<GetChangeOfProviderLinkResponse.ChangeOfProviderLink> ChangeOfProviderChain { get; set; }
        public IReadOnlyCollection<ApprenticeshipOverlappingTrainingDateRequest> OverlappingTrainingDateRequest { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }

        public class GetApprenticeshipResponse
        {
            public long Id { get; set; }
            public long CohortId { get; set; }
            public long ProviderId { get; set; }
            public string ProviderName { get; set; }
            public long EmployerAccountId { get; set; }
            public long AccountLegalEntityId { get; set; }
            public string EmployerName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Uln { get; set; }
            public string NINumber { get; set; }
            public string CourseCode { get; set; }
            public string StandardUId { get; set; }
            public string Version { get; set; }
            public string Option { get; set; }
            public string CourseName { get; set; }
            public DeliveryModel DeliveryModel { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? ActualStartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string EmployerReference { get; set; }
            public string ProviderReference { get; set; }
            public ApprenticeshipStatus Status { get; set; }
            public DateTime? StopDate { get; set; }
            public DateTime? PauseDate { get; set; }
            public DateTime? CompletionDate { get; set; }
            public string EndpointAssessorName { get; set; }
            public bool HasHadDataLockSuccess { get; set; }
            public long? ContinuationOfId { get; set; }
            public long? ContinuedById { get; set; }
            public DateTime? OriginalStartDate { get; set; }
            public bool IsContinuation => ContinuationOfId.HasValue;
            public bool HasContinuation => ContinuedById.HasValue;
            public long? PreviousProviderId { get; set; }
            public long? PreviousEmployerAccountId { get; set; }
            public ApprenticeshipEmployerType? ApprenticeshipEmployerTypeOnApproval { get; set; }
            public bool? MadeRedundant { get; set; }
            public ConfirmationStatus? ConfirmationStatus { get; set; }
            public bool EmailAddressConfirmedByApprentice { get; set; }
            public bool EmailShouldBePresent { get; set; }
            public int? PledgeApplicationId { get; set; }
            public int? EmploymentPrice { get; set; }
            public DateTime? EmploymentEndDate { get; set; }
            public bool? RecognisePriorLearning { get; set; }
            public int? TrainingTotalHours { get; set; }
            public int? DurationReducedByHours { get; set; }
            public bool? IsDurationReducedByRpl { get; set; }
            public int? DurationReducedBy { get; set; }
            public int? PriceReducedBy { get; set; }
            public long? TransferSenderId { get; set; }
            public bool? IsOnFlexiPaymentPilot { get; set; }
        }


        public class GetPriceEpisodeResponse
        {
            public class PriceEpisode
            {
                public long Id { get; set; }
                public long ApprenticeshipId { get; set; }
                public decimal Cost { get; set; }
                public DateTime FromDate { get; set; }
                public DateTime? ToDate { get; set; }
            }

            public IReadOnlyCollection<PriceEpisode> PriceEpisodes { get; set; }
        }

        public class GetApprenticeshipUpdateResponse
        {
            public class ApprenticeshipUpdate
            {
                public long Id { get; set; }
                public long ApprenticeshipId { get; set; }
                public Party OriginatingParty { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Email { get; set; }
                public DeliveryModel? DeliveryModel { get; set; }
                public DateTime? EmploymentEndDate { get; set; }
                public int? EmploymentPrice { get; set; }
                public ProgrammeType? TrainingType { get; set; }
                public string TrainingCode { get; set; }
                public string Version { get; set; }
                public string Option { get; set; }
                public string TrainingName { get; set; }
                public decimal? Cost { get; set; }
                public DateTime? StartDate { get; set; }
                public DateTime? EndDate { get; set; }
                public DateTime? DateOfBirth { get; set; }
            }
            public IReadOnlyCollection<ApprenticeshipUpdate> ApprenticeshipUpdates { get; set; }
        }


        public class GetDataLockResponse
        {
            public class DataLock
            {
                public long Id { get; set; }
                public DateTime DataLockEventDatetime { get; set; }
                public string PriceEpisodeIdentifier { get; set; }
                public long ApprenticeshipId { get; set; }
                public string IlrTrainingCourseCode { get; set; }
                public DateTime? IlrActualStartDate { get; set; }
                public DateTime? IlrEffectiveFromDate { get; set; }
                public DateTime? IlrPriceEffectiveToDate { get; set; }
                public decimal? IlrTotalCost { get; set; }
                public DataLockErrorCode ErrorCode { get; set; }
                public Status DataLockStatus { get; set; }
                public TriageStatus TriageStatus { get; set; }
                public bool IsResolved { get; set; }
            }
            public IReadOnlyCollection<DataLock> DataLocks { get; set; }
        }


        public class GetChangeOfPartyRequestResponse
        {
            public class ChangeOfPartyRequest
            {
                public long Id { get; set; }
                public ChangeOfPartyRequestType ChangeOfPartyType { get; set; }
                public Party OriginatingParty { get; set; }
                public ChangeOfPartyRequestStatus Status { get; set; }
                public string EmployerName { get; set; }
                public DateTime? StartDate { get; set; }
                public DateTime? EndDate { get; set; }
                public int? Price { get; set; }
                public long? CohortId { get; set; }
                public Party? WithParty { get; set; }
                public long? NewApprenticeshipId { get; set; }
                public long? ProviderId { get; set; }
            }
            public IReadOnlyCollection<ChangeOfPartyRequest> ChangeOfParty { get; set; }
        }


        public class GetChangeOfProviderLinkResponse
        {
            public class ChangeOfProviderLink
            {
                public long ApprenticeshipId { get; set; }
                public string ProviderName { get; set; }
                public DateTime? StartDate { get; set; }
                public DateTime? EndDate { get; set; }
                public DateTime? StopDate { get; set; }
                public DateTime? CreatedOn { get; set; }
            }

            public IReadOnlyCollection<ChangeOfProviderLink> ChangeOfProviderChain { get; set; }
        }

        public class ApprenticeshipOverlappingTrainingDateRequest
        {
            public long Id { get; set; }
            public long DraftApprenticeshipId { get; set; }
            public long PreviousApprenticeshipId { get; set; }
            public OverlappingTrainingDateRequestResolutionType? ResolutionType { get; set; }
            public OverlappingTrainingDateRequestStatus Status { get; set; }
            public DateTime? ActionedOn { get; set; }
        }
    }
}