using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class IndexRequest : IAuthorizationContextModel
    {
        [FromRoute]
        public string AccountHashedId { get; set; }
        public Guid? ReservationId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }

        public string StartMonthYear { get; set; }
        public DeliveryModel? DeliveryModel { get; set; }
        public string CourseCode { get; set; }
    }
}