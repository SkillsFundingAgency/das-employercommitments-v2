﻿using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.PaymentOrder;

public class WhenCallingGetPaymentOrder
{
    [Test, MoqAutoData]
    public async Task Then_Returns_View_With_Correct_Model(
        PaymentOrderRequest request,
        PaymentOrderViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] PaymentOrderController controller)
    {
        mockMapper
            .Setup(mapper => mapper.Map<PaymentOrderViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.ProviderPaymentOrder(request) as ViewResult;

        result.ViewName.Should().BeNull();
        var model = result.Model as PaymentOrderViewModel;
        model.Should().BeSameAs(viewModel);
    }
}