using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
[Parallelizable]
public class PaymentOrderViewModelValidatorTests : ValidatorTestBase<PaymentOrderViewModel, PaymentOrderViewModelValidator>
{
    [Test]
    public void Validate_Should_Be_False_When_Empty_ProviderPaymentOrder()
    {
        var model = new PaymentOrderViewModel
        {
            ProviderPaymentOrder = []
        };

        AssertValidationResult(request => request.ProviderPaymentOrder, model, false, "'Provider Payment Order' must not be empty.");
    }
    
    [Test]
    public void Validate_Should_Be_True_When_Unique_ProviderPaymentOrder()
    {
        var model = new PaymentOrderViewModel
        {
            ProviderPaymentOrder =
            [
                "10000001",
                "10000002",
                "10000003",
                "10000004",
                "10000005",
            ]
        };

        AssertValidationResult(request => request.ProviderPaymentOrder, model, true);
    }
    
    [Test]
    public void Validate_Should_Be_False_When_Not_Unique_ProviderPaymentOrder()
    {
        var model = new PaymentOrderViewModel
        {
            ProviderPaymentOrder =
            [
                "10000001",
                "10000002",
                "10000003",
                "10000004",
                "10000004",
            ]
        };

        AssertValidationResult(request => request.ProviderPaymentOrder, model, false, "Each training provider can only appear once in the list");
    }
}