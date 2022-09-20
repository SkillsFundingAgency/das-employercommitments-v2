using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses
{
    public class GetEditApprenticeshipResponse
    {
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public bool IsFundedByTransfer { get; set; }
        public string CourseName { get; set; }
        public bool HasUnavailableDeliveryModel { get; set; }
        public DeliveryModel DeliveryModel { get; set; }
    }
}
