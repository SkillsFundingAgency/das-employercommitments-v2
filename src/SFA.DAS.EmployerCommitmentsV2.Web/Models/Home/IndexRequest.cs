using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Home
{
    public class IndexRequest : IAuthorizationContextModel
    {
        [FromRoute]
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
    }
}
