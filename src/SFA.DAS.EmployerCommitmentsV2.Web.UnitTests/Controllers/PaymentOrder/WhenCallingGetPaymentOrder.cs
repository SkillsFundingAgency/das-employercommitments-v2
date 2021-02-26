using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.PaymentOrderControllerTests

{
    public class WhenCallingGetPaymentOrder
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_View_With_Correct_Model(
            PaymentOrderRequest request,
            PaymentOrderViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            PaymentOrderController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<PaymentOrderViewModel>(request))
                .ReturnsAsync(viewModel);

            var result = await controller.PaymentOrder(request) as ViewResult;

            result.ViewName.Should().BeNull();
            var model = result.Model as PaymentOrderViewModel;
            model.Should().BeSameAs(viewModel);
        }
    }
}