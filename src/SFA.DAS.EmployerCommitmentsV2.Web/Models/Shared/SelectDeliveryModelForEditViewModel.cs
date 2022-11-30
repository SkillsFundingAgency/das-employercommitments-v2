using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared
{
    public class SelectDeliveryModelForEditViewModel : IDeliveryModelSelection
    {
        public List<EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel> DeliveryModels { get; set; }
        public EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel? DeliveryModel { get; set; }
        public string LegalEntityName { get; set; }
        public string CourseCode { get; set; }
        public bool ShowTrainingDetails { get; set; }
    }
}