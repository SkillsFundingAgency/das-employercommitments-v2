using SFA.DAS.CommitmentsV2.Api.Types.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public interface IDraftApprenticeshipRequest
    {
        DetailsRequest Request { get; }
        GetCohortResponse Cohort { get; }
    }
}
