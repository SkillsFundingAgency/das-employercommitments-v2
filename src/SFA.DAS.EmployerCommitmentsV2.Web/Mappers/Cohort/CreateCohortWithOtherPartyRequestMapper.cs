using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class CreateCohortWithOtherPartyRequestMapper(IEncodingService encodingService) : IMapper<AddApprenticeshipCacheModel, CreateCohortWithOtherPartyRequest>
{
    public Task<CreateCohortWithOtherPartyRequest> Map(AddApprenticeshipCacheModel source)
    {
        long? decodedTransferSenderId = null;
        int? decodedPledgeApplicationId = null;

        if (!string.IsNullOrEmpty(source.TransferSenderId))
        {
            encodingService.TryDecode(source.TransferSenderId, EncodingType.PublicAccountId, out var tempTransferSenderId);
            decodedTransferSenderId = tempTransferSenderId == 0 ? null : tempTransferSenderId;
        }

        if (!string.IsNullOrEmpty(source.EncodedPledgeApplicationId))
        {
            encodingService.TryDecode(source.EncodedPledgeApplicationId, EncodingType.PledgeApplicationId, out var tempPledgeApplicationId);
            decodedPledgeApplicationId = tempPledgeApplicationId == 0 ? null : (int?)tempPledgeApplicationId;
        }     

        return Task.FromResult(new CreateCohortWithOtherPartyRequest
        {
            AccountId = source.AccountId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            ProviderId = source.ProviderId,
            Message = source.Message,
            TransferSenderId = decodedTransferSenderId,
            PledgeApplicationId = decodedPledgeApplicationId
        });
    }
}