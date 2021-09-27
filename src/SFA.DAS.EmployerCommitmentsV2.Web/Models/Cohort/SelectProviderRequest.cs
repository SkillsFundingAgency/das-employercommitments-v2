using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class SelectProviderRequest : IndexRequest
    {
        public string TransferSenderId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public Origin Origin { get; set; }

        [FromQuery]
        public string EncodedPledgeApplicationId { get; set; }
    }
}