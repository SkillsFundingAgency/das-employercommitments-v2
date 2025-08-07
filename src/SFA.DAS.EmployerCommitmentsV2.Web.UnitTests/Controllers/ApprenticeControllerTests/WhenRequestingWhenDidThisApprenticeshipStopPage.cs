using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingWhenDidThisApprenticeshipStopPage : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockModelMapper = new Mock<IModelMapper>();
        MockCookieStorageService = new Mock<Interfaces.ICookieStorageService<IndexRequest>>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        CacheStorageService = new Mock<ICacheStorageService>();
        ApprovalsApiClient = new Mock<IApprovalsApiClient>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            MockCookieStorageService.Object,
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>(),
            ApprovalsApiClient.Object);
    }

    [Test]
    public async Task WhenRequesting_WhenDidThisApprenticeshipStop_ThenStopRequestViewModelIsPassedToTheView()
    {
        var requestViewModel = new StopRequestViewModel { StopMonth = 6, StopYear = 2020, ApprenticeshipId = 1, AccountHashedId = "AAXX" };
        MockModelMapper.Setup(m => m.Map<StopRequestViewModel>(It.IsAny<StopRequest>()))
            .ReturnsAsync(requestViewModel);

        var viewResult = await Controller.StopApprenticeship(new StopRequest()) as ViewResult;
        var viewModel = viewResult.Model;

        var stopRequestViewModel = (StopRequestViewModel)viewModel;

        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<StopRequestViewModel>());
            Assert.That(stopRequestViewModel, Is.EqualTo(requestViewModel));
        });
    }
}