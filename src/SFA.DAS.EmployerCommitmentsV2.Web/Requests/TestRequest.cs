using SFA.DAS.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Requests
{
    public class TestRequest : IAuthorizationContextModel
    {
        public string CohortReference { get; set; }
        public long? CohortId { get; set; }

        public string AccountHashedId { get; set; }
        public long? AccountId { get; set; }
    }
}
