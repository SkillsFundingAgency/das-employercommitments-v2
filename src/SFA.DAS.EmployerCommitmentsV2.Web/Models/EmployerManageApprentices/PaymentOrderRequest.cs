using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.EmployerManageApprentices
{
    public class PaymentOrderRequest : IAuthorizationContextModel
    {
        [FromRoute]
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
    }
}