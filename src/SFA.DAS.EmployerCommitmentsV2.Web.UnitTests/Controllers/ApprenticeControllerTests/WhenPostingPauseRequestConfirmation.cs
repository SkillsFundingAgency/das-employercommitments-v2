using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingPauseRequestConfirmation : ApprenticeControllerTestBase
{
    private const string ApprenticePausedMessage = "Apprenticeship paused";
    private const string FlashMessageBody = nameof(FlashMessageBody);
    private const string FlashMessageLevel = nameof(FlashMessageLevel);
    private const string FlashMessageTitle = nameof(FlashMessageTitle);

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
        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public async Task AndConfirmPauseIsSelected_ThenCommitmentsApiPauseApprenticeshipIsCalled(PauseRequestViewModel request)
    {  
        //Act
        await Controller.PauseApprenticeship(request);

        //Assert
        MockCommitmentsApiClient.Verify(p => p.PauseApprenticeship(It.IsAny<PauseApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndConfirmPauseIsSelected_ThenRedirectToApprenticeDetailsPage(PauseRequestViewModel request)
    {
        //Arrange
        request.PauseConfirmed = true;

        //Act
        var result = await Controller.PauseApprenticeship(request) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    [Test, MoqAutoData]
    public async Task AndConfirmPauseIsSelected_ThenRedirectToApprenticeDetailsPageWithFlashMessage(PauseRequestViewModel request)
    {
        //Arrange
        request.PauseConfirmed = true;

        //Act
        var result = await Controller.PauseApprenticeship(request) as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(Controller.TempData.Values.Contains(ApprenticePausedMessage), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitle), Is.True);
        });
    }

    [Test, MoqAutoData]
    public async Task AndGoBackIsSelected_ThenCommitmentsApiPauseApprenticeshipIsNotCalled(PauseRequestViewModel request)
    {
        //Arrange
        request.PauseConfirmed = false;

        //Act
        await Controller.PauseApprenticeship(request);

        //Assert
        MockCommitmentsApiClient.Verify(p => p.PauseApprenticeship(It.IsAny<PauseApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPage(PauseRequestViewModel request)
    {
        //Arrange
        request.PauseConfirmed = false;

        //Act
        var result = await Controller.PauseApprenticeship(request) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    [Test, MoqAutoData]
    public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPageWithoutFlashMessage(PauseRequestViewModel request)
    {
        //Arrange
        request.PauseConfirmed = false;

        //Act
        var result = await Controller.PauseApprenticeship(request) as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(Controller.TempData.Values.Contains(ApprenticePausedMessage), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitle), Is.False);
        });
    }
}