using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class PaymentOrderViewModelValidator : AbstractValidator<PaymentOrderViewModel>
    {
        public PaymentOrderViewModelValidator()
        {
            RuleFor(x => x.ProviderPaymentOrder)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(x =>
                {
                    var hasDuplicates = x.GroupBy(y => y).Any(g => g.Count() > 1);
                    return !hasDuplicates;
                }).WithMessage("Each training provider can only appear once in the list");
        }
    }
}
