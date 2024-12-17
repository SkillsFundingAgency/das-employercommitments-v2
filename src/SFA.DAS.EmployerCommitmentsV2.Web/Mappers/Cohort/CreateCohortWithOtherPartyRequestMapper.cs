using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class CreateCohortWithOtherPartyRequestMapper(IEncodingService encodingService) : IMapper<AddApprenticeshipCacheModel, CreateCohortWithOtherPartyRequest>
{
    public Task<CreateCohortWithOtherPartyRequest> Map(AddApprenticeshipCacheModel source)
    {
        encodingService.TryDecode(source.TransferSenderId, EncodingType.PublicAccountId, out var decodedTransferSenderId);
        encodingService.TryDecode(source.TransferSenderId, EncodingType.PledgeApplicationId, out var decodedPledgeApplicationId);

        return Task.FromResult(new CreateCohortWithOtherPartyRequest
        {
            AccountId = source.AccountId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            ProviderId = source.ProviderId,
            Message = source.Message,
            TransferSenderId = decodedTransferSenderId == 0 ? null : decodedTransferSenderId,
            PledgeApplicationId = decodedPledgeApplicationId == 0 ? null : (int?)decodedPledgeApplicationId
        });
    }
}