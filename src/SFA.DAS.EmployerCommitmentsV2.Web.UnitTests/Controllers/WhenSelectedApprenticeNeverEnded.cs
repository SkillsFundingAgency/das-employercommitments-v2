using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers;

public class WhenSelectedApprenticeNeverEnded:ApprenticeControllerTestBase
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

    [Test, MoqAutoData]
    public async Task WhenSelected_ApprenticeshipNotEnded_ThenApprenticeshipNotEndedViewModelIsPassedToTheView(ApprenticeshipNotEndedViewModel expectedViewModel)
    {
        MockModelMapper.Setup(m => m.Map<ApprenticeshipNotEndedViewModel>(It.IsAny<ApprenticeshipNotEndedRequest>()))
            .ReturnsAsync(expectedViewModel);

        var viewResult = await Controller.ApprenticeshipNotEnded(new ApprenticeshipNotEndedRequest()) as ViewResult;
        var viewModel = viewResult.Model;

        var actualViewModel = (ApprenticeshipNotEndedViewModel)viewModel;

        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<ApprenticeshipNotEndedViewModel>());
            Assert.That(actualViewModel, Is.EqualTo(expectedViewModel));
        });
    }
}