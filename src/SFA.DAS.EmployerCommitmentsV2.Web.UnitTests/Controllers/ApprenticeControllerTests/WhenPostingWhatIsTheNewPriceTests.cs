﻿using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingWhatIsTheNewPriceTests
{
    private Fixture _autoFixture;
    private WhenPostingWhatIsTheNewPriceTestsFixture _fixture;
    private WhatIsTheNewPriceViewModel _viewModel;

    [SetUp]
    public void Arrange()
    {
        _autoFixture = new Fixture();
        _fixture = new WhenPostingWhatIsTheNewPriceTestsFixture();
        _viewModel = _autoFixture.Build<WhatIsTheNewPriceViewModel>().Create();
    }   

    [Test]
    public async Task AndUserIsChangingTheirAnswer_ThenRedirectToTheConfirmationPage()
    {
        _viewModel.Edit = true;

        var result = await _fixture.WhatIsTheNewPrice(_viewModel);

        WhenPostingWhatIsTheNewPriceTestsFixture.VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(result as RedirectToRouteResult);
    }
}

public class WhenPostingWhatIsTheNewPriceTestsFixture : ApprenticeControllerTestFixtureBase
{
    public async Task<IActionResult> WhatIsTheNewPrice(WhatIsTheNewPriceViewModel viewModel)
    {
        return await Controller.WhatIsTheNewPrice(viewModel);
    }     

    public static void VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(IActionResult result)
    {
        var redirectResult = (RedirectToRouteResult)result;

        Assert.That(redirectResult.RouteName, Is.EqualTo(RouteNames.ConfirmDetailsAndSendRequest));
    }
}