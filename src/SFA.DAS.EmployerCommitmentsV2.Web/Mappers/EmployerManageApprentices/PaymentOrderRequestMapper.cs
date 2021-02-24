using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.EmployerManageApprentices;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.EmployerManageApprentices
{
    public class PaymentOrderRequestMapper : IMapper<PaymentOrderViewModel, PaymentOrderRequest>
    {
        public Task<PaymentOrderRequest> Map(PaymentOrderViewModel source)
        {
            return Task.FromResult(new PaymentOrderRequest
            {
                AccountHashedId = source.AccountHashedId
            });
        }
    }
}
