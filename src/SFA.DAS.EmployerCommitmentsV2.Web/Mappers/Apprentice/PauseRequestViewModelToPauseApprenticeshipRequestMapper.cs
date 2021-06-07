using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class PauseRequestViewModelToPauseApprenticeshipRequestMapper : IMapper<PauseRequestViewModel, PauseApprenticeshipRequest>
    {
        public Task<PauseApprenticeshipRequest> Map(PauseRequestViewModel source)
        {
            return Task.FromResult(new PauseApprenticeshipRequest
            {
                ApprenticeshipId = source.ApprenticeshipId
            });
        }
    }
}
