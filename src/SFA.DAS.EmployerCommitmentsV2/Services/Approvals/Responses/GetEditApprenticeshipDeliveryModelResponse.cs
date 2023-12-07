using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses
{
    public class GetEditApprenticeshipDeliveryModelResponse
    {
        public string LegalEntityName { get; set; }
        public DeliveryModel DeliveryModel { get; set; }
        public List<DeliveryModel> DeliveryModels { get; set; }
    }
}
