using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class CreateChangeOfPartyRequestMapper : IMapper<SendNewTrainingProviderViewModel, CreateChangeOfPartyRequestRequest>
    {
        public Task<CreateChangeOfPartyRequestRequest> Map(SendNewTrainingProviderViewModel source)
        {
            return Task.FromResult(new CreateChangeOfPartyRequestRequest
            {                
                ChangeOfPartyRequestType = ChangeOfPartyRequestType.ChangeProvider,
                NewPartyId = source.ProviderId
            });
        }
    }
}
