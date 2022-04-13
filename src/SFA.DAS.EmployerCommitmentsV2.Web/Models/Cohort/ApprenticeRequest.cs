using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ApprenticeRequest : AssignRequest
    {
        public long AccountId { get; set; }
        public bool AutoCreated { get; set; }
        public DeliveryModel? DeliveryModel { get; set; }
    }
}