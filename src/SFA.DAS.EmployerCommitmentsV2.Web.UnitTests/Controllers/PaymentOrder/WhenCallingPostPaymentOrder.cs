using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.PaymentOrderControllerTests
{
    public class WhenCallingPostPaymentOrder
    {
        [Test, MoqAutoData]
        public async Task And_UpdateProviderPaymentsPriority_Succeeds_Then_Redirect_To_Home(
            Mock<ICommitmentsApiClient> mockCommitmentsApiClient,
            UpdateProviderPaymentsPriorityRequest request,
            PaymentOrderViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            PaymentOrderController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<UpdateProviderPaymentsPriorityRequest>(viewModel))
                .ReturnsAsync(request);

            mockCommitmentsApiClient
                .Setup(r => r.UpdateProviderPaymentsPriority(
                    It.IsAny<long>(),
                    It.IsAny<UpdateProviderPaymentsPriorityRequest>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var expectedRouteValues = new RouteValueDictionary(new
            {
                viewModel.AccountHashedId
            });

            var result = (await controller.ProviderPaymentOrder(viewModel)) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");
            result.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
        }

        [Test, MoqAutoData]
        public async Task And_UpdateProviderPaymentsPriority_Fails_Then_Redirect_To_Error(
            PaymentOrderViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            PaymentOrderController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<UpdateProviderPaymentsPriorityRequest>(viewModel))
                .ThrowsAsync(new Exception("Some error"));

            var result = (await controller.ProviderPaymentOrder(viewModel)) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Error");
            result.ControllerName.Should().Be("Error");
            result.RouteValues.Should().BeNull();
        }
    }
}