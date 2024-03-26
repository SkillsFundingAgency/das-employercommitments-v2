using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingPauseApprenticeshipConfirmationPage : ApprenticeControllerTestBase
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

    [Test]
    public async Task AndCurrentStatusIsLive_ThenViewIsReturned()
    {
        MockModelMapper.Setup(m => m.Map<PauseRequestViewModel>(It.IsAny<PauseRequest>()))
            .ReturnsAsync(new PauseRequestViewModel());

        var result = await Controller.PauseApprenticeship(new PauseRequest());

        Assert.That(result, Is.InstanceOf<ViewResult>());
    }

    [Test, MoqAutoData]
    public async Task AndCurrentStatusIsLive_ThenPauseRequestViewModelIsPassedToTheView(PauseRequestViewModel _viewModel)
    {
        MockModelMapper.Setup(m => m.Map<PauseRequestViewModel>(It.IsAny<PauseRequest>()))
            .ReturnsAsync(_viewModel);

        var viewResult = await Controller.PauseApprenticeship(new PauseRequest()) as ViewResult;
        var viewModel = viewResult.Model;

        var pauseRequestViewModel = (PauseRequestViewModel)viewModel;

        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<PauseRequestViewModel>());
            Assert.That(pauseRequestViewModel, Is.EqualTo(_viewModel));
        });
    }
}