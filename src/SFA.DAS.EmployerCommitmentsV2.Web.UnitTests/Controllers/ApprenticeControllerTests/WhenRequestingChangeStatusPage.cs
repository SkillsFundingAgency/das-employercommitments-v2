using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingChangeStatusPage : ApprenticeControllerTestBase
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
        MockModelMapper.Setup(m => m.Map<ChangeStatusRequestViewModel>(It.IsAny<ChangeStatusRequest>()))
            .ReturnsAsync(new ChangeStatusRequestViewModel { CurrentStatus = ApprenticeshipStatus.Live });

        var result = await Controller.ChangeStatus(new ChangeStatusRequest());

        Assert.That(result, Is.InstanceOf<ViewResult>());
    }
}