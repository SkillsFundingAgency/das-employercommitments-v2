using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetProviderPaymentsPriorityResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.PaymentOrder;

public class PaymentOrderViewModelMapper : IMapper<PaymentOrderRequest, PaymentOrderViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public PaymentOrderViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<PaymentOrderViewModel> Map(PaymentOrderRequest source)
    {
        var response = await _commitmentsApiClient.GetProviderPaymentsPriority(source.AccountId);
            
        var viewModel = MapPayment(response.ProviderPaymentPriorities.ToList());
        viewModel.AccountHashedId = source.AccountHashedId;
            
        return viewModel;
    }

    private PaymentOrderViewModel MapPayment(IList<ProviderPaymentPriorityItem> data)
    {
        var items = data.Select(m => new PaymentOrderItem
            {
                ProviderId = m.ProviderId,
                ProviderName = m.ProviderName,
                Priority = m.PriorityOrder
            })
            .OrderBy(m => m.Priority);

        return new PaymentOrderViewModel(items);
    }
}