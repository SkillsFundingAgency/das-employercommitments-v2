using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingReconfirmHasNotStopPage : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockModelMapper = new Mock<IModelMapper>();
        MockCookieStorageService = new Mock<ICookieStorageService<IndexRequest>>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            MockCookieStorageService.Object,
            MockCommitmentsApiClient.Object,
            Mock.Of<ILogger<ApprenticeController>>());
    }

    [Test, MoqAutoData]
    public async Task WhenRequesting_ReconfirmApprenticeshipHasNotStop_ThenReConfirmHasNotStopRequestViewModelIsPassedToTheView(ReconfirmHasNotStopViewModel expectedViewModel)
    {
        MockModelMapper
            .Setup(m => m.Map<ReconfirmHasNotStopViewModel>(It.IsAny<ReConfirmHasNotStopRequest>()))
            .ReturnsAsync(expectedViewModel);

        var viewResult = await Controller.ReconfirmHasNotStop(new ReConfirmHasNotStopRequest()) as ViewResult;
        var viewModel = viewResult.Model;

        var actualViewModel = (ReconfirmHasNotStopViewModel)viewModel;

        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<ReconfirmHasNotStopViewModel>());
            Assert.That(actualViewModel, Is.EqualTo(expectedViewModel));
        });
    }
}