using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ResumeRequestViewModelToResumeApprenticeshipRequestMapper : IMapper<ResumeRequestViewModel, ResumeApprenticeshipRequest>
{
    public Task<ResumeApprenticeshipRequest> Map(ResumeRequestViewModel source)
    {
        return Task.FromResult(new ResumeApprenticeshipRequest
        {
            ApprenticeshipId = source.ApprenticeshipId
        });
    }
}