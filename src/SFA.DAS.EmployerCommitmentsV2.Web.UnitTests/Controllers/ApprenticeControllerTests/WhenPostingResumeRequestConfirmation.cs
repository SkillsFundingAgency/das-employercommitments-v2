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
public class WhenPostingResumeRequestConfirmation : ApprenticeControllerTestBase
{
    private const string ApprenticeResumeMessage = "Apprenticeship resumed";
    private const string FlashMessageBody = nameof(FlashMessageBody);
    private const string FlashMessageLevel = nameof(FlashMessageLevel);
    private const string FlashMessageTitle = nameof(FlashMessageTitle);

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
        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public async Task AndConfirmResumeIsSelected_ThenCommitmentsApiResumeApprenticeshipIsCalled(ResumeRequestViewModel request)
    {
        //Act
        await Controller.ResumeApprenticeship(request);

        //Assert
        MockCommitmentsApiClient.Verify(p => 
            p.ResumeApprenticeship(It.IsAny<ResumeApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndConfirmResumeIsSelected_ThenRedirectToApprenticeDetailsPage(ResumeRequestViewModel request)
    {
        //Arrange
        request.ResumeConfirmed = true;

        //Act
        var result = await Controller.ResumeApprenticeship(request) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    [Test, MoqAutoData]
    public async Task AndConfirmResumeIsSelected_ThenRedirectToApprenticeDetailsPageWithFlashMessage(ResumeRequestViewModel request)
    {
        //Arrange
        request.ResumeConfirmed = true;

        //Act
        var result = await Controller.ResumeApprenticeship(request) as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(Controller.TempData.Values.Contains(ApprenticeResumeMessage), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitle), Is.True);
        });
    }

    [Test, MoqAutoData]
    public async Task AndGoBackIsSelected_ThenCommitmentsApiResumeApprenticeshipIsNotCalled(ResumeRequestViewModel request)
    {
        //Arrange
        request.ResumeConfirmed = false;

        //Act
        await Controller.ResumeApprenticeship(request);

        //Assert
        MockCommitmentsApiClient.Verify(p => p.ResumeApprenticeship(It.IsAny<ResumeApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPage(ResumeRequestViewModel request)
    {
        //Arrange
        request.ResumeConfirmed = false;

        //Act
        var result = await Controller.ResumeApprenticeship(request) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    [Test, MoqAutoData]
    public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPageWithoutFlashMessage(ResumeRequestViewModel request)
    {
        //Arrange
        request.ResumeConfirmed = false;

        //Act
        var result = await Controller.ResumeApprenticeship(request) as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(Controller.TempData.Values.Contains(ApprenticeResumeMessage), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitle), Is.False);
        });
    }
}