using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.TransferRequestControllerTests
{
    public class WhenCallingPostTransferDetailsForSender
    {
        [Test, MoqAutoData]
        public async Task And_TransferDetailsForSender_Succeeds_Then_Redirect_To_Confirmation(
            UpdateTransferApprovalForSenderRequest request,
            TransferRequestForSenderViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Greedy] TransferRequestController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<UpdateTransferApprovalForSenderRequest>(viewModel))
                .ReturnsAsync(request);

            var result = (await controller.TransferDetailsForSender(viewModel)) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("TransferConfirmation");
            result.ControllerName.Should().Be("TransferRequest");
        }

        [Test, MoqAutoData]
        public async Task And_TransferDetailsForSender_Succeeds_Then_Api_Called_To_Update(
            Mock<ICommitmentsApiClient> mockCommitmentsApiClient,
            Mock<ILogger<TransferRequestController>> mockLogger,
            UpdateTransferApprovalForSenderRequest request,
            TransferRequestForSenderViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper)
        {
            mockMapper
                .Setup(mapper => mapper.Map<UpdateTransferApprovalForSenderRequest>(viewModel))
                .ReturnsAsync(request);

            mockCommitmentsApiClient
                .Setup(r => r.UpdateTransferRequestForSender(
                    It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<UpdateTransferApprovalForSenderRequest>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            TransferRequestController controller = new TransferRequestController(mockCommitmentsApiClient.Object,
                mockLogger.Object, mockMapper.Object);

            var result = (await controller.TransferDetailsForSender(viewModel)) as RedirectToActionResult;

            mockCommitmentsApiClient.Verify(m => m.UpdateTransferRequestForSender(
                It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<UpdateTransferApprovalForSenderRequest>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_TransferDetailsForSender_Fails_Then_Redirect_To_Error(
            TransferRequestForSenderViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Greedy] TransferRequestController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<UpdateTransferApprovalForSenderRequest>(viewModel))
                .ThrowsAsync(new Exception("Some error"));

            var result = (await controller.TransferDetailsForSender(viewModel)) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Error");
            result.ControllerName.Should().Be("Error");
            result.RouteValues.Should().BeNull();
        }
    }
}