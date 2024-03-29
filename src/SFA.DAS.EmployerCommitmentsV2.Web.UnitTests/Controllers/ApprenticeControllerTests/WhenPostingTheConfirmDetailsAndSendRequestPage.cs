﻿using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingTheConfirmDetailsAndSendRequestPage 
{
    private WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture();
    }

    [Test]
    public async Task VerifyCommitmentsApiCreateChangeOfPartyRequestCalled()
    {
        await _fixture.ConfirmDetailsAndSendRequest();

        _fixture.VerifyCommitmentsApiCreateChangeOfPartyRequestCalled();
    }

    [Test]
    public async Task And_CommitmentsApiSuccessful_Then_RedirectToConfirmationPage()
    {
        var actionResult = await _fixture.ConfirmDetailsAndSendRequest();

        _fixture.VerifyRedirectsToSentAction(actionResult);
    }

    [Test]
    public async Task And_CommitmentsApiReturnsError_Then_RedirectToError()
    {
        _fixture.SetErrorFromCommitmentsApi();

        var actionResult = await _fixture.ConfirmDetailsAndSendRequest();

        WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture.VerifyRedirectToError(actionResult);
    }
}

public class WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ConfirmDetailsAndSendViewModel _viewModel;

    public WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture()
    {
        _viewModel = AutoFixture.Build<ConfirmDetailsAndSendViewModel>()
            .With(x => x.NewStartDate, new DateTime(2020, 1, 1))
            .With(x => x.NewEndDate, new DateTime(2022, 1, 1))
            .Create();
    }
    public async Task<IActionResult> ConfirmDetailsAndSendRequest()
    {
        return await Controller.ConfirmDetailsAndSendRequestPage(_viewModel);
    }

    public WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture SetErrorFromCommitmentsApi()
    {
        MockCommitmentsApiClient.Setup(c => c.CreateChangeOfPartyRequest(It.IsAny<long>(), It.IsAny<CreateChangeOfPartyRequestRequest>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        return this;
    }

    public void VerifyCommitmentsApiCreateChangeOfPartyRequestCalled()
    {
        MockCommitmentsApiClient.Verify(p => p.CreateChangeOfPartyRequest(It.IsAny<long>(), It.IsAny<CreateChangeOfPartyRequestRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    public void VerifyRedirectsToSentAction(IActionResult result)
    {
        var redirect = (RedirectToRouteResult)result;

        Assert.That(redirect.RouteName, Is.EqualTo(RouteNames.ChangeProviderRequestedConfirmation));

        var routeValues = redirect.RouteValues;

        Assert.Multiple(() =>
        {
            Assert.That(routeValues["ProviderId"], Is.EqualTo(_viewModel.ProviderId));
            Assert.That(routeValues["ApprenticeshipHashedId"], Is.EqualTo(_viewModel.ApprenticeshipHashedId));
            Assert.That(routeValues["AccountHashedId"], Is.EqualTo(_viewModel.AccountHashedId));
            Assert.That(routeValues["StoppedDuringCoP"], Is.EqualTo(_viewModel.StoppedDuringCoP));
        });
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