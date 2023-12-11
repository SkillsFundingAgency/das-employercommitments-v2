using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingWhatIsTheNewEndDateTests
{
    private Fixture _autoFixture;
    private WhenPostingWhatIsTheNewEndDateTestsFixture _fixture;
    private WhatIsTheNewEndDateViewModel _viewModel;

    [SetUp]
    public void Arrange()
    {
        _autoFixture = new Fixture();
        _fixture = new WhenPostingWhatIsTheNewEndDateTestsFixture();
        _viewModel = _autoFixture.Build<WhatIsTheNewEndDateViewModel>().Create();
    }

    [Test]
    public async Task ThenRedirectToTheWhatIsTheNewStopDatePage()
    {
        _viewModel.Edit = false;

        var result = await _fixture.WhatIsTheNewEndDate(_viewModel);

        WhenPostingWhatIsTheNewEndDateTestsFixture.VerifyRedirectsToTheWhatIsTheNewPricePage(result as RedirectToRouteResult);
    }

    [Test]
    public async Task AndUserIsChangingTheirAnswer_ThenRedirectToTheConfirmationPage()
    {
        _viewModel.Edit = true;

        var result = await _fixture.WhatIsTheNewEndDate(_viewModel);

        WhenPostingWhatIsTheNewEndDateTestsFixture.VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(result as RedirectToRouteResult);
    }
}

public class WhenPostingWhatIsTheNewEndDateTestsFixture : ApprenticeControllerTestFixtureBase
{
    public async Task<IActionResult> WhatIsTheNewEndDate(WhatIsTheNewEndDateViewModel viewModel)
    {
        return await Controller.WhatIsTheNewEndDate(viewModel);
    }

    public static void VerifyRedirectsToTheWhatIsTheNewPricePage(IActionResult result)
    {
        var redirectResult = (RedirectToRouteResult)result;

        Assert.That(redirectResult.RouteName, Is.EqualTo(RouteNames.WhatIsTheNewPrice));
    }

    public static void VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(IActionResult result)
    {
        var redirectResult = (RedirectToRouteResult)result;

        Assert.That(redirectResult.RouteName, Is.EqualTo(RouteNames.ConfirmDetailsAndSendRequest));
    }
}