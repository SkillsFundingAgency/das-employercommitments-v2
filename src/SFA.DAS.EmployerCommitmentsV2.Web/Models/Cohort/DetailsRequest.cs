using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsRequest : IAuthorizationContextModel
    {
        [FromRoute]
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        [FromRoute]
        public string CohortReference { get; set; }
        public long CohortId { get; set; }
    }
}
