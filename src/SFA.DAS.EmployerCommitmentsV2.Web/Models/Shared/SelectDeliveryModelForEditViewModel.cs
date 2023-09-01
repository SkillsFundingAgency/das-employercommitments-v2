using System.Collections.Generic;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared
{
    public class SelectDeliveryModelForEditViewModel : IDeliveryModelSelection
    {
        public List<DeliveryModel> DeliveryModels { get; set; }
        public DeliveryModel? DeliveryModel { get; set; }
        public string LegalEntityName { get; set; }
        public string CourseCode { get; set; }
        public bool HasUnavailableFlexiJobAgencyDeliveryModel { get; set; }
        public bool ShowFlexiJobAgencyDeliveryModelConfirmation { get; set; }

        public string PageTitle => ShowFlexiJobAgencyDeliveryModelConfirmation
            ? "Confirm the apprenticeship delivery model"
            : "Select the apprenticeship delivery model";
    }
}