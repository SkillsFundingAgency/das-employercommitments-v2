using System;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class MessageViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public string ReservationId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
    }
}