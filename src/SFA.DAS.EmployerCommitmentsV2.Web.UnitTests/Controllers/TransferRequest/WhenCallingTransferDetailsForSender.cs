using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.TransferRequest;

public class WhenCallingTransferDetailsForSender
{
    [Test, MoqAutoData]
    public async Task Then_Returns_View_With_Correct_Model(
        TransferRequestRequest request,
        TransferRequestForSenderViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] TransferRequestController controller)
    {
        mockMapper
            .Setup(mapper => mapper.Map<TransferRequestForSenderViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.TransferDetailsForSender(request) as ViewResult;

        result.ViewName.Should().BeNull();
        var model = result.Model as TransferRequestForSenderViewModel;
        model.Should().BeSameAs(viewModel);
    }
}