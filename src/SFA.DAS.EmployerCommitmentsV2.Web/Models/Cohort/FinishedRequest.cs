using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class FinishedRequest : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public string CohortReference { get; set; }
        public long CohortId { get; set; }
        
    }
}