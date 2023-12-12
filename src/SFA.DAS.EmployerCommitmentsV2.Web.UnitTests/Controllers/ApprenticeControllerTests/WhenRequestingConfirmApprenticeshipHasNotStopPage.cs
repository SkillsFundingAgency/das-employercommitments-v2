using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingConfirmApprenticeshipHasNotStopPage : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockModelMapper = new Mock<IModelMapper>();
        MockCookieStorageService = new Mock<Interfaces.ICookieStorageService<IndexRequest>>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            MockCookieStorageService.Object,
            MockCommitmentsApiClient.Object,
            Mock.Of<ILogger<ApprenticeController>>());
    }

    [Test, MoqAutoData]
    public async Task WhenRequesting_ConfirmApprenticeshipHasNotStop_ThenConfirmHasNotStopRequestViewModelIsPassedToTheView(ConfirmHasNotStopViewModel expectedViewModel)
    {
        MockModelMapper
            .Setup(m => m.Map<ConfirmHasNotStopViewModel>(It.IsAny<ConfirmHasNotStopRequest>()))
            .ReturnsAsync(expectedViewModel);

        var viewResult = await Controller.ConfirmHasNotStop(new ConfirmHasNotStopRequest()) as ViewResult;
        var viewModel = viewResult.Model;

        var actualViewModel = (ConfirmHasNotStopViewModel)viewModel;

        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<ConfirmHasNotStopViewModel>());
            Assert.That(actualViewModel, Is.EqualTo(expectedViewModel));
        });
    }
}