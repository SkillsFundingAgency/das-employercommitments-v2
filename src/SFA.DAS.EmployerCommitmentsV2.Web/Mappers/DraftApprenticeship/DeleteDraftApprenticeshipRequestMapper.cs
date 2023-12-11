using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class DeleteDraftApprenticeshipRequestMapper : IMapper<DeleteDraftApprenticeshipViewModel, DeleteDraftApprenticeshipRequest>
{
    public Task<DeleteDraftApprenticeshipRequest> Map(DeleteDraftApprenticeshipViewModel source)
    {
        return Task.FromResult(new DeleteDraftApprenticeshipRequest());
    }
}