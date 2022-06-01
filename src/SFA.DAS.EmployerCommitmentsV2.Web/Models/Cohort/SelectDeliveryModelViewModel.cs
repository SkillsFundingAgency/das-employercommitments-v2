using System.Collections.Generic;
using SFA.DAS.CommitmentsV2.Types;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class SelectDeliveryModelViewModel
    {
        public string AccountHashedId { get; set; }
        public List<DeliveryModelMapped> DeliveryModels { get; set; }

        public Guid? ReservationId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public string LegalEntityName { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public long ProviderId { get; set; }
        public string TransferSenderId { get; set; }
        public string EncodedPledgeApplicationId { get; set; }
        public int DeliveryModel { get; set; }
    }

    public class DeliveryModelMapped
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
