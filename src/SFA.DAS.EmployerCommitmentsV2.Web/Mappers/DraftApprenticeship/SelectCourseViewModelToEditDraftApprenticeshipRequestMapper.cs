using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class SelectCourseViewModelToEditDraftApprenticeshipRequestMapper : IMapper<SelectCourseViewModel, EditDraftApprenticeshipRequest>
{
    public Task<EditDraftApprenticeshipRequest> Map(SelectCourseViewModel source)
    {
        return Task.FromResult(new EditDraftApprenticeshipRequest
        {
            //AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            AccountHashedId = source.AccountHashedId,
            CohortId = source.CohortId,
            CohortReference = source.CohortReference,
            CourseCode = source.CourseCode,
            DeliveryModel = source.DeliveryModel,
            DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
            //ProviderId = source.ProviderId,
            //ReservationId = source.ReservationId.HasValue ? source.ReservationId.Value : System.Guid.Empty,
            //StartMonthYear = source.StartMonthYear
        });
    }
}