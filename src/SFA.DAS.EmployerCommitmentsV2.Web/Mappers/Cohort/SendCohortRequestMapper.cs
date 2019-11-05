using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class SendCohortRequestMapper : IMapper<DetailsViewModel, SendCohortRequest>
    {
        public Task<SendCohortRequest> Map(DetailsViewModel source)
        {
            return Task.FromResult(new SendCohortRequest
            {
                Message = source.SendMessage
            });
        }
    }
}
