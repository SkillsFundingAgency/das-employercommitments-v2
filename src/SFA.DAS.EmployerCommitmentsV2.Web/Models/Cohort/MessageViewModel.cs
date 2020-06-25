using SFA.DAS.Authorization.ModelBinding;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class MessageViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public Guid? ReservationId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public string LegalEntityName { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
        public string TransferSenderId { get; set; }
        public long? DecodedTransferSenderId { get; set; }
    }
}