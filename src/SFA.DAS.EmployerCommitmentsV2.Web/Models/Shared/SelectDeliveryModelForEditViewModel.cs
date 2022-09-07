using System;
using System.Collections.Generic;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared
{
    public class SelectDeliveryModelForEditViewModel : IDeliveryModelSelection
    {
        public List<EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel> DeliveryModels { get; set; }
        public EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel? DeliveryModel { get; set; }
        public string LegalEntityName { get; set; }
    }
}