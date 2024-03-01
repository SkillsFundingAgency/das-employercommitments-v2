using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class MessageViewModelMapper : IMapper<MessageRequest, MessageViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public MessageViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<MessageViewModel> Map(MessageRequest source)
    {
        var provider = await _commitmentsApiClient.GetProvider(source.ProviderId);

        return new MessageViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = source.LegalEntityName,
            ProviderId = source.ProviderId,
            StartMonthYear = source.StartMonthYear,
            CourseCode = source.CourseCode,
            ReservationId = source.ReservationId,
            ProviderName = provider.Name,
            TransferSenderId = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId
        };
    }
}