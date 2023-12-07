
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EmployerLedCreateChangeOfProviderRequestMapper : IMapper<ConfirmDetailsAndSendViewModel, CreateChangeOfPartyRequestRequest>
{
    public Task<CreateChangeOfPartyRequestRequest> Map(ConfirmDetailsAndSendViewModel source)
    {
            return Task.FromResult(new CreateChangeOfPartyRequestRequest
            {
                ChangeOfPartyRequestType = ChangeOfPartyRequestType.ChangeProvider,
                NewPartyId = source.ProviderId.Value,
                NewStartDate = source.NewStartDate,
                NewEndDate = source.NewEndDate,
                NewPrice = source.NewPrice
            });
        }
}