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
public class WhenPostingConfirmStopApprenticeship : ApprenticeControllerTestBase
{
    private const string ApprenticeStoppedMessage = "Apprenticeship stopped";
    private const string FlashMessageBody = "FlashMessageBody";
    private const string FlashMessageTitle = "FlashMessageTitle";
    private const string FlashMessageLevel = "FlashMessageLevel";

    [SetUp]
    public void Arrange()
    {
        var fixture = new Fixture();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        MockModelMapper = new Mock<IModelMapper>();
        CacheStorageService = new Mock<Interfaces.ICacheStorageService>();

        var stopApprenticeshipRequest = fixture.Create<StopApprenticeshipRequest>();

        MockModelMapper.Setup(m => m.Map<StopApprenticeshipRequest>(It.IsAny<ConfirmStopRequestViewModel>()))
            .ReturnsAsync(stopApprenticeshipRequest);

        Controller = new ApprenticeController(MockModelMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>());
        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsStopped_AndIsCoPJourney_ThenRedirectToEnterNewTrainingProvider(ConfirmStopRequestViewModel viewModel)
    {
        var result = await Controller.ConfirmStop(viewModel);

        var redirect =  result.VerifyReturnsRedirectToActionResult();
        
        Assert.Multiple(() =>
        {
            Assert.That(redirect.ActionName, Is.EqualTo("ApprenticeshipStoppedInform"));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));
        });
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsStopped_CommitmentApi_IsCalled(ConfirmStopRequestViewModel viewModel)
    {
        //Arrange
        viewModel.StopConfirmed = true;

        //Act
        await Controller.ConfirmStop(viewModel);

        //Assert
        MockCommitmentsApiClient.Verify(x => x.StopApprenticeship(viewModel.ApprenticeshipId, It.IsAny<StopApprenticeshipRequest>(), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsNotStopped_CommitmentApi_IsNotCalled(ConfirmStopRequestViewModel viewModel)
    {
        //Arrange
        viewModel.StopConfirmed = false;

        //Act
        await Controller.ConfirmStop(viewModel);

        //Assert
        MockCommitmentsApiClient.Verify(x => x.StopApprenticeship(It.IsAny<long>(), It.IsAny<StopApprenticeshipRequest>(), CancellationToken.None), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsStopped_and_Mapper_IsCalled(ConfirmStopRequestViewModel viewModel)
    {
        //Arrange
        viewModel.StopConfirmed = true;

        //Act
        await Controller.ConfirmStop(viewModel);

        //Assert
        MockModelMapper.Verify(x => x.Map<StopApprenticeshipRequest>(viewModel), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsNotStopped__and_Mapper_IsNotCalled(ConfirmStopRequestViewModel viewModel)
    {
        //Arrange
        viewModel.StopConfirmed = false;

        //Act
        await Controller.ConfirmStop(viewModel);

        //Assert
        MockModelMapper.Verify(x => x.Map<StopApprenticeshipRequest>(viewModel), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsNotStopped_And_IsCopJourney_ThenRedirectsToApprenticeshipDetails(ConfirmStopRequestViewModel viewModel)
    {
        //Arrange
        viewModel.StopConfirmed = false;

        //Act
        var result = await Controller.ConfirmStop(viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsNotStopped_And_IsNotCopJourney_ThenRedirectsToApprenticeshipDetails(ConfirmStopRequestViewModel viewModel)
    {
        //Arrange
        viewModel.StopConfirmed = false;

        //Act
        var result = await Controller.ConfirmStop(viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsStopped_ThenRedirectToApprenticeDetailsPageWithFlashMessage(ConfirmStopRequestViewModel request)
    {
        //Arrange
        request.StopConfirmed = true;
        request.IsCoPJourney = false;

        //Act
        var result = await Controller.ConfirmStop(request) as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(Controller.TempData.Values.Contains(ApprenticeStoppedMessage), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitle), Is.True);
        });
    }

    [Test, MoqAutoData]
    public async Task AndApprenticeshipIsStoppedDuringChangeOfProvider_ThenSetStoppedDuringChangeOfProvider_IsTrue(ConfirmStopRequestViewModel viewModel)
    {
        viewModel.StopConfirmed = true;
        viewModel.IsCoPJourney = true;
        var result = await Controller.ConfirmStop(viewModel);

        var redirectResult = (RedirectToActionResult)result;
        var routeValues = redirectResult.RouteValues;

        Assert.Multiple(() =>
        {
            Assert.That(routeValues["StoppedDuringCoP"], Is.EqualTo(true));
            Assert.That(Controller.TempData.Values.Contains(ApprenticeStoppedMessage), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitle), Is.False);
        });
    }
}