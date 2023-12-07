using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.PaymentOrder;

public class UpdateProviderPaymentsPriorityRequestMapper : IMapper<PaymentOrderViewModel, UpdateProviderPaymentsPriorityRequest>
{
    private readonly IAuthenticationService _authenticationService;

    public UpdateProviderPaymentsPriorityRequestMapper(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<UpdateProviderPaymentsPriorityRequest> Map(PaymentOrderViewModel source)
    {
        return await Task.FromResult(new UpdateProviderPaymentsPriorityRequest
        {
            ProviderPriorities = source.ProviderPaymentOrder
                .Select((p, index) => new UpdateProviderPaymentsPriorityRequest.ProviderPaymentPriorityUpdateItem
                {
                    ProviderId = long.Parse(p),
                    PriorityOrder = index + 1
                })
                .ToList(),
            UserInfo = _authenticationService.UserInfo
        });
    }
}