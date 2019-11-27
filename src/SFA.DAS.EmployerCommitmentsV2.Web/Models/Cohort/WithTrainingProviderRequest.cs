using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class WithTrainingProviderRequest : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }

    }
}
