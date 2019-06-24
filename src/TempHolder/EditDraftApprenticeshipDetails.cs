using System;

namespace SFA.DAS.Commitments.Shared.Extensions
{
    public class EditDraftApprenticeshipDetails
    {
        public long DraftApprenticeshipId { get; set; }
        public string DraftApprenticeshipHashedId { get; set; }
        public long CohortId { get; set; }
        public int ProviderId { get; set; }
        public string CohortReference { get; set; }
        public Guid? ReservationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string UniqueLearnerNumber { get; set; }
        public string CourseCode { get; set; }
        public int? Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OriginatorReference { get; set; }
    }
}