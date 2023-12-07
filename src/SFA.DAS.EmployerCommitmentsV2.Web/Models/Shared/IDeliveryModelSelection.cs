using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

public interface IDeliveryModelSelection
{
    List<DeliveryModel> DeliveryModels { get; set; }
    DeliveryModel? DeliveryModel { get; set; }
}