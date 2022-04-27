using System;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public class AddDraftApprenticeshipRequest : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string CohortReference { get; set; }
        public long CohortId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public Guid ReservationId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public bool AutoCreated { get; set; }
        public long ProviderId { get; set; }
        public DeliveryModel? DeliveryModel { get; set; }
        public int? Cost { get; set; }
        public int? EmploymentPrice { get; set; }
        public DateTime? EmploymentEndDate { get;  set; }
    }
}