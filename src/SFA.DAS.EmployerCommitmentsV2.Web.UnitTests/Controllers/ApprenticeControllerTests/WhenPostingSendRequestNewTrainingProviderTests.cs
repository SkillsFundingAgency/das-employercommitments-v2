using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingSendRequestNewTrainingProviderTests
{
    private WhenPostingSendRequestNewTrainingProviderTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenPostingSendRequestNewTrainingProviderTestsFixture();
    }

    [Test]
    public async Task VerifyRedirectsToApprenticeDetailsPage()
    {
        //Arrange
        _fixture.SetConfirm(false);

        //Act
        var result = await _fixture.SendRequestNewTrainingProvider();

        //Assert
        WhenPostingSendRequestNewTrainingProviderTestsFixture.VerifyRedirectsToApprenticeDetailsPage(result);
    }

    [Test]
    public async Task VerifyRedirectsToSentAction()
    {
        //Arrange
        _fixture.SetConfirm(true);

        //Act
        var result = await _fixture.SendRequestNewTrainingProvider();

        //Assert
        WhenPostingSendRequestNewTrainingProviderTestsFixture.VerifyRedirectsToSentAction(result);
    }

    [Test]
    public async Task VerifyCommitmentsApiCreateChangeOfPartyRequestCalled()
    {
        //Arrange
        _fixture.SetConfirm(true);

        //Act
        await _fixture.SendRequestNewTrainingProvider();

        //Assert
        _fixture.VerifyCommitmentsApiCreateChangeOfPartyRequestCalled();
    }

    [Test]
    public async Task And_NewProviderIsTheSameAsCurrentProvider_Then_RedirectToError()
    {
        _fixture.SetErrorFromCommitmentsApi();

        var actionResult = await _fixture.SendRequestNewTrainingProvider();

        WhenPostingSendRequestNewTrainingProviderTestsFixture.VerifyRedirectToError(actionResult);
    }

    [Test]
    public async Task RedirectingToConfirmationPage_Set_ProviderAddDetails_ToTrue()
    {
        _fixture.SetConfirm(true);

        var actionResult = (RedirectToRouteResult)await _fixture.SendRequestNewTrainingProvider();

        Assert.Multiple(() =>
        {
            Assert.That(RouteNames.ChangeProviderRequestedConfirmation, Is.EqualTo(actionResult.RouteName));
            Assert.That(true, Is.EqualTo(actionResult.RouteValues[nameof(ChangeProviderRequestedConfirmationRequest.ProviderAddDetails)]));
        });
    }
}

public class WhenPostingSendRequestNewTrainingProviderTestsFixture
{
    private readonly Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private ApprenticeController _controller;
    private SendNewTrainingProviderViewModel _viewModel;

    public WhenPostingSendRequestNewTrainingProviderTestsFixture()
    {
        var autoFixture = new Fixture();
        _viewModel = autoFixture.Create<SendNewTrainingProviderViewModel>();

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _controller = new ApprenticeController(Mock.Of<IModelMapper>(),
            Mock.Of<ICookieStorageService<IndexRequest>>(),
            _commitmentsApiClient.Object,
            Mock.Of<ILogger<ApprenticeController>>());
    }

    public async Task<IActionResult> SendRequestNewTrainingProvider()
    {
        return await _controller.SendRequestNewTrainingProvider(_viewModel);
    }

    public WhenPostingSendRequestNewTrainingProviderTestsFixture SetConfirm(bool confirm)
    {
        _viewModel.Confirm = confirm;
        return this;
    }

    public WhenPostingSendRequestNewTrainingProviderTestsFixture SetErrorFromCommitmentsApi()
    {
        _commitmentsApiClient.Setup(c => c.CreateChangeOfPartyRequest(It.IsAny<long>(), It.IsAny<CreateChangeOfPartyRequestRequest>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        return this;
    }

    public static void VerifyRedirectsToApprenticeDetailsPage(IActionResult result)
    {
        var redirect = (RedirectToActionResult)result;
        Assert.That(redirect.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    public static void VerifyRedirectsToSentAction(IActionResult result)
    {
        var redirect = (RedirectToRouteResult)result;
        Assert.That(redirect.RouteName, Is.EqualTo(RouteNames.ChangeProviderRequestedConfirmation));
    }

    public void VerifyCommitmentsApiCreateChangeOfPartyRequestCalled()
    {
        _commitmentsApiClient.Verify(p => p.CreateChangeOfPartyRequest(It.IsAny<long>(), It.IsAny<CreateChangeOfPartyRequestRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    public static void VerifyRedirectToError(IActionResult actionResult)
    {
        var redirectResult = actionResult as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Error"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Error"));
        });
    }
}