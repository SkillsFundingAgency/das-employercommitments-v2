using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class MessageViewModelToCreateCohortWithOtherPartyRequestMapper : IMapper<MessageViewModel, CreateCohortWithOtherPartyRequest>
    {
        public CreateCohortWithOtherPartyRequest Map(MessageViewModel source)
        {
            return new CreateCohortWithOtherPartyRequest
            {
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                ProviderId = source.ProviderId,
                Message = source.Message
            };
        }
    }
}
