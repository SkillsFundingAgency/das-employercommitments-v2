using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class EditDraftApprenticeshipRequest: BaseEditDraftApprenticeshipRequest, IDraftApprenticeshipRequest, IAuthorizationContextModel
{
    public DetailsRequest Request { get; set; }
    public GetCohortResponse Cohort { get; set; }
}

public class BaseEditDraftApprenticeshipRequest
{
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }
    public string CohortReference { get; set; }
    public long CohortId { get; set; }
    public string AccountLegalEntityHashedId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string DraftApprenticeshipHashedId { get; set; }
    public string CourseCode { get; set; }
    public DeliveryModel? DeliveryModel { get; set; }

    public BaseEditDraftApprenticeshipRequest CloneBaseValues() =>
        this.ExplicitClone();
}