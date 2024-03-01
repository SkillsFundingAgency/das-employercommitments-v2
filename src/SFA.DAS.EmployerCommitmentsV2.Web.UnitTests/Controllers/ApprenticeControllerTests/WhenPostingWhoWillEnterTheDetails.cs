using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingWhoWillEnterTheDetails
{
    private Fixture _autoFixture;
    private WhenPostingWhoWillEnterTheDetailsTestFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _autoFixture = new Fixture();

        _fixture = new WhenPostingWhoWillEnterTheDetailsTestFixture();
    }

    [Test]
    public async Task AndEmployerIsSelected_ThenRedirectToWhatIsTheNewStartDateRoute()
    {
        var viewModel = _autoFixture.Build<WhoWillEnterTheDetailsViewModel>()
            .With(vm => vm.EmployerWillAdd, true)
            .Create();

        var result = await _fixture.WhoWillEnterTheDetails(viewModel);

        WhenPostingWhoWillEnterTheDetailsTestFixture.VerifyRedirectsToWhatIsTheNewStartDateRoute(result);
    }

    [Test]
    public async Task AndProviderIsSelected_ThenRedirectToSendNewTrainingProviderRequest()
    {
        var viewModel = _autoFixture.Build<WhoWillEnterTheDetailsViewModel>()
            .With(vm => vm.EmployerWillAdd, false)
            .Create();

        var result = await _fixture.WhoWillEnterTheDetails(viewModel);

        WhenPostingWhoWillEnterTheDetailsTestFixture.VerifyRedirectsToSendRequestNewTrainingProviderRoute(result);
    }
}

public class WhenPostingWhoWillEnterTheDetailsTestFixture : ApprenticeControllerTestFixtureBase
{
    public async Task<IActionResult> WhoWillEnterTheDetails(WhoWillEnterTheDetailsViewModel viewModel)
    {
        return await Controller.WhoWillEnterTheDetails(viewModel);
    }

    public static void VerifyRedirectsToWhatIsTheNewStartDateRoute(IActionResult result)
    {
        var redirectResult = (RedirectToRouteResult)result;

        Assert.That(redirectResult.RouteName, Is.EqualTo(RouteNames.WhatIsTheNewStartDate));
    }

    public static void VerifyRedirectsToSendRequestNewTrainingProviderRoute(IActionResult result)
    {
        var redirectResult = (RedirectToRouteResult)result;

        Assert.That(redirectResult.RouteName, Is.EqualTo(RouteNames.SendRequestNewTrainingProvider));
    }
}