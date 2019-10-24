using SFA.DAS.CommitmentsV2.Api.Types.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public class ViewDraftApprenticeshipRequest
    {
        public DetailsRequest Request { get; set; }
        public GetCohortResponse Cohort { get; set; }
    }
}
