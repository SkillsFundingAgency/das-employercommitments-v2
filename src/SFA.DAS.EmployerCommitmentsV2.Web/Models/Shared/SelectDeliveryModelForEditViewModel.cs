using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared
{
    public class SelectDeliveryModelForEditViewModel : IDeliveryModelSelection
    {
        public List<EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel> DeliveryModels { get; set; }
        public EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel? DeliveryModel { get; set; }
        public string LegalEntityName { get; set; }
        public bool HasUnavailableFlexiJobAgencyDeliveryModel { get; set; }
        public bool ShowFlexiJobAgencyDeliveryModelConfirmation { get; set; }

        public string PageTitle => ShowFlexiJobAgencyDeliveryModelConfirmation
            ? "Confirm the apprenticeship delivery model"
            : "Select the apprenticeship delivery model";
    }
}