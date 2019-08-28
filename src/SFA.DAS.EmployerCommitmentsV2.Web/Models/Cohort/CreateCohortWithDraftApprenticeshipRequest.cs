using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class CreateCohortWithDraftApprenticeshipRequest : AssignRequest, IAuthorizationContextModel
    {
        public long AccountLegalEntityId { get; set; }
    }
}