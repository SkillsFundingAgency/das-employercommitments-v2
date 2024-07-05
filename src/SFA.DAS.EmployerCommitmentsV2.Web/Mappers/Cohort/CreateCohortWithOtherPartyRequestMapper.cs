using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class CreateCohortWithOtherPartyRequestMapper : IMapper<AssignViewModel, CreateCohortWithOtherPartyRequest>
{
    public Task<CreateCohortWithOtherPartyRequest> Map(AssignViewModel source)
    {
        return Task.FromResult(new CreateCohortWithOtherPartyRequest
        {
            AccountId = source.AccountId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            ProviderId = source.ProviderId,
            Message = source.SendMessage,
            TransferSenderId = source.DecodedTransferSenderId,
            PledgeApplicationId = (int?) source.PledgeApplicationId
        });
    }
}