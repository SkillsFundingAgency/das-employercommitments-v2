using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetEditDraftApprenticeshipSelectDeliveryModelResponse
{
    public string EmployerName { get; set; }
    public DeliveryModel? DeliveryModel { get; set; }
    public List<DeliveryModel> DeliveryModels { get; set; }
    public bool HasUnavailableDeliveryModel { get; set; }
}