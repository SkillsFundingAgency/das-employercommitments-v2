using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;

public class PaymentOrderViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }
    public IEnumerable<PaymentOrderItem> PaymentOrderItems { get; set; }
    public string[] ProviderPaymentOrder { get; set; } = default;

    public PaymentOrderViewModel()
    {
    }

    public PaymentOrderViewModel(IEnumerable<PaymentOrderItem> items)
    {
        PaymentOrderItems = items;
            
        ProviderPaymentOrder = PaymentOrderItems?
            .OrderBy(o => o.Priority)
            .Select(p => p.ProviderId.ToString())
            .ToArray() ?? default;
    }

    public List<SelectListItem> ProviderSelectListItems()
    {
        return PaymentOrderItems?
            .OrderBy(o => o.Priority)
            .Select(p => new SelectListItem { Value = p.ProviderId.ToString(), Text = p.ProviderName })
            .ToList();
    }
}

public class PaymentOrderItem
{
    public string ProviderName { get; set; }

    public long ProviderId { get; set; }

    public int Priority { get; set; }
}