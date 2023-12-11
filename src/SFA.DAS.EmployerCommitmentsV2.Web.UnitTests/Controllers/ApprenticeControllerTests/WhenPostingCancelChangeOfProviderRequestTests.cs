using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingCancelChangeOfProviderRequestTests
{
    private Fixture _autoFixture;
    private WhenPostingCancelChangeOfProviderRequestTestsFixture _fixture;
    private CancelChangeOfProviderRequestViewModel _viewModel;

    [SetUp]
    public void Arrange()
    {
        _autoFixture = new Fixture();

        _viewModel = _autoFixture.Build<CancelChangeOfProviderRequestViewModel>().Create();

        _fixture = new WhenPostingCancelChangeOfProviderRequestTestsFixture();
    }

    [Test]
    public async Task AndYesIsSelected_ThenRedirectToApprenticeDetailsPage()
    {
        _viewModel.CancelRequest = true;
            
        var result = await _fixture.CancelChangeOfProviderRequest(_viewModel);

        WhenPostingCancelChangeOfProviderRequestTestsFixture.VerifyRedirectsToApprenticeDetailsPage(result as RedirectToRouteResult);
    }

    [Test]
    public async Task AndNoIsSelected_ThenRedirectToConfirmAndSendPage()
    {
        _viewModel.CancelRequest = false;

        var result = await _fixture.CancelChangeOfProviderRequest(_viewModel);

        _fixture.VerifyRedirectsToConfirmAndSendPage(result as RedirectToRouteResult);
    }
}

public class WhenPostingCancelChangeOfProviderRequestTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ChangeOfProviderRequest _request;
      
    public WhenPostingCancelChangeOfProviderRequestTestsFixture() : base() 
    {
        _request = AutoFixture.Build<ChangeOfProviderRequest>().Create();
        MockMapper.Setup(m => m.Map<ChangeOfProviderRequest>(It.IsAny<CancelChangeOfProviderRequestViewModel>())).ReturnsAsync(_request);
    }

    public async Task<IActionResult> CancelChangeOfProviderRequest(CancelChangeOfProviderRequestViewModel viewModel)
    {
        return await Controller.CancelChangeOfProviderRequest(viewModel);
    }

    public static void VerifyRedirectsToApprenticeDetailsPage(IActionResult result)
    {
        var redirectResult = (RedirectToRouteResult)result;
        Assert.That(redirectResult.RouteName, Is.EqualTo(RouteNames.ApprenticeDetail));
    }

    public void VerifyRedirectsToConfirmAndSendPage(IActionResult result)
    {
        var redirectResult = (RedirectToRouteResult)result;

        Assert.That(redirectResult.RouteName, Is.EqualTo(RouteNames.ConfirmDetailsAndSendRequest));

        var routeValues = redirectResult.RouteValues;

        Assert.Multiple(() =>
        {
            Assert.That(routeValues["ProviderId"], Is.EqualTo(_request.ProviderId));
            Assert.That(routeValues["ApprenticeshipHashedId"], Is.EqualTo(_request.ApprenticeshipHashedId));
            Assert.That(routeValues["AccountHashedId"], Is.EqualTo(_request.AccountHashedId));
            Assert.That(routeValues["NewStartMonth"], Is.EqualTo(_request.NewStartMonth));
            Assert.That(routeValues["NewStartYear"], Is.EqualTo(_request.NewStartYear));
            Assert.That(routeValues["NewEndMonth"], Is.EqualTo(_request.NewEndMonth));
            Assert.That(routeValues["NewEndYear"], Is.EqualTo(_request.NewEndYear));
            Assert.That(routeValues["NewPrice"], Is.EqualTo(_request.NewPrice));
        });
    }
}