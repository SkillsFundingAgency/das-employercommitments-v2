using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class SelectDeliveryModelViewModelToEditDraftApprenticeshipViewModelMapper : IMapper<SelectDeliveryModelViewModel, EditDraftApprenticeshipViewModel>
{
    public Task<EditDraftApprenticeshipViewModel> Map(SelectDeliveryModelViewModel source)
    {
        return Task.FromResult(new EditDraftApprenticeshipViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ProviderId = source.ProviderId,
            CohortId = source.CohortId,
            CohortReference = source.CohortReference,
            DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
            ReservationId = source.ReservationId,
            CourseCode = source.CourseCode,
            DeliveryModel = source.DeliveryModel,
        });
    }
}