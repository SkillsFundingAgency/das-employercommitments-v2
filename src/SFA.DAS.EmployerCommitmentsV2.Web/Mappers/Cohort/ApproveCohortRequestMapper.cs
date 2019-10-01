using System.Threading.Tasks;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ApproveCohortRequestMapper : IMapper<DetailsViewModel, ApproveCohortRequest>
    {
            public Task<ApproveCohortRequest> Map(DetailsViewModel source)
            {
                return Task.FromResult(new ApproveCohortRequest
                {
                    Message = source.ApproveMessage
                });
            }
    }
}
