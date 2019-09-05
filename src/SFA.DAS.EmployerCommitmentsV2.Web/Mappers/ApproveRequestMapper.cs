using System.Threading.Tasks;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class ApproveRequestMapper : IMapper<ApproveRequest, ApproveViewModel>
    {
        public Task<ApproveViewModel> Map(ApproveRequest source)
        {
            return Task.FromResult(new ApproveViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference
            });
        }
    }
}