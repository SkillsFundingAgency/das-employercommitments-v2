using System.Threading.Tasks;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class MessageViewModelToCreateCohortWithOtherPartyRequestMapper : IMapper<MessageViewModel, CreateCohortWithOtherPartyRequest>
    {
        public Task<CreateCohortWithOtherPartyRequest> Map(MessageViewModel source)
        {
            return Task.FromResult(new CreateCohortWithOtherPartyRequest
            {
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                ProviderId = source.ProviderId,
                Message = source.Message
            });
        }
    }
}
