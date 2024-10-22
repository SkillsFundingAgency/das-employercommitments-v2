using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingResumeApprenticeshipConfirmationPage : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockModelMapper = new Mock<IModelMapper>();
        MockCookieStorageService = new Mock<Interfaces.ICookieStorageService<IndexRequest>>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        CacheStorageService = new Mock<ICacheStorageService>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            MockCookieStorageService.Object,
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>());
    }

    [Test]
    public async Task AndCurrentStatusIsPaused_ThenViewIsReturned()
    {
        var resumedDate = DateTime.Now;
        MockModelMapper.Setup(m => m.Map<ResumeRequestViewModel>(It.IsAny<ResumeRequest>()))
            .ReturnsAsync(new ResumeRequestViewModel()
            {
                ResumeDate = resumedDate
            });

        var result = await Controller.ResumeApprenticeship(new ResumeRequest());

        var viewResult = (ViewResult)result;
        var viewModel = viewResult.Model;
        
        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.TypeOf<ResumeRequestViewModel>());
            Assert.That(result, Is.InstanceOf<ViewResult>());
        });
    }
}