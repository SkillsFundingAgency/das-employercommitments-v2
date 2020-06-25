using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ApprenticeRequest : AssignRequest
    {
        public long AccountId { get; set; }
        public bool AutoCreated { get; set; }
    }
}