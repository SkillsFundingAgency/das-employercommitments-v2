using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

public class EditApprenticeshipDeliveryModelViewModel : IDeliveryModelSelection
{
    public List<EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel> DeliveryModels { get; set; }
    public EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel? DeliveryModel { get; set; }
    public string LegalEntityName { get; set; }
    public Guid CacheKey { get; set; }
}