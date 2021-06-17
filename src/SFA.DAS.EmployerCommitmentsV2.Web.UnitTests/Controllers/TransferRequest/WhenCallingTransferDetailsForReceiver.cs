using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.TransferRequestControllerTests

{
    public class WhenCallingTransferDetailsForReceiver
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_View_With_Correct_Model(
            TransferRequestRequest request,
            TransferRequestForReceiverViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            TransferRequestController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<TransferRequestForReceiverViewModel>(request))
                .ReturnsAsync(viewModel);

            var result = await controller.TransferDetailsForReceiver(request) as ViewResult;

            result.ViewName.Should().BeNull();
            var model = result.Model as TransferRequestForReceiverViewModel;
            model.Should().BeSameAs(viewModel);
        }
    }
}