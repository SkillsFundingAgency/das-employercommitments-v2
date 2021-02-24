using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.EmployerManageApprentices;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.EmployerManageApprenticesControllerTests
{
    public class WhenCallingPostPaymentOrder
    {
        [Test, MoqAutoData]
        public async Task And_UpdateProviderPaymentsPriority_Succeeds_Then_Redirect_To_Home(
            Mock<IAuthenticationService> mockAuthenticationService,
            Mock<ICommitmentsApiClient> mockCommitmentsApiClient,
            EmployerManageApprenticesController controller)
        {
            PaymentOrderViewModel viewModel = new PaymentOrderViewModel
            {
                ProviderPaymentOrder = new string[]
                {
                    "12345678",
                    "23456789",
                    "34567890"
                }
            };

            mockCommitmentsApiClient
                .Setup(r => r.UpdateProviderPaymentsPriority(
                    It.IsAny<long>(),
                    It.IsAny<UpdateProviderPaymentsPriorityRequest>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockAuthenticationService
                .SetupGet(p => p.UserInfo)
                .Returns(new UserInfo
                {
                    UserId = Guid.NewGuid().ToString(),
                    UserDisplayName = "Test, Tester",
                    UserEmail = "tester@test.com"
                });

            var expectedRouteValues = new RouteValueDictionary(new
            {
                viewModel.AccountHashedId
            });

            var result = (await controller.PaymentOrder(mockAuthenticationService.Object, viewModel)) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");
            result.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
        }

        [Test, MoqAutoData]
        public async Task And_UpdateProviderPaymentsPriority_Fails_Then_Redirect_To_Error(
            Mock<IAuthenticationService> mockAuthenticationService,
            EmployerManageApprenticesController controller)
        {
            PaymentOrderViewModel viewModel = new PaymentOrderViewModel
            {
                ProviderPaymentOrder = new string[]
                {
                    "not a number"
                }
            };

            mockAuthenticationService
                .SetupGet(p => p.UserInfo)
                .Returns(new UserInfo
                {
                    UserId = Guid.NewGuid().ToString(),
                    UserDisplayName = "Test, Tester",
                    UserEmail = "tester@test.com"
                });

            var result = (await controller.PaymentOrder(mockAuthenticationService.Object, viewModel)) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Error");
            result.ControllerName.Should().Be("Error");
            result.RouteValues.Should().BeNull();
        }
    }
}