using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingWhatIsTheNewStartDateTests
{
    private Fixture _autoFixture;
    private WhenPostingWhatIsTheNewStartDateTestFixture _fixture;

    private WhatIsTheNewStartDateViewModel _viewModel;

    [SetUp]
    public void Arrange()
    {
        _autoFixture = new Fixture();

        _viewModel = _autoFixture.Build<WhatIsTheNewStartDateViewModel>().Create();

        _fixture = new WhenPostingWhatIsTheNewStartDateTestFixture();
    }

    [Test]
    public async Task ThenRedirectToTheWhatIsTheNewStopDatePage()
    {
        _viewModel.Edit = false;
        _fixture.SetupAdvanceToStopDate();

        var result = await _fixture.WhatIsTheNewStartDate(_viewModel);

        _fixture.VerifyRedirectsToTheWhatIsTheNewEndDatePage(result as RedirectToRouteResult);
    }

    [Test]
    public async Task AndUserIsChangingTheirAnswer_ThenRedirectToTheConfirmationPage()
    {
        _viewModel.Edit = true;
        _fixture.SetupReturnToCheckYourAnswers();
        var result = await _fixture.WhatIsTheNewStartDate(_viewModel);

        _fixture.VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(result as RedirectToRouteResult);
    }
}

public class WhenPostingWhatIsTheNewStartDateTestFixture : ApprenticeControllerTestFixtureBase
{
    private ChangeOfProviderRequest _request;

    public async Task<IActionResult> WhatIsTheNewStartDate(WhatIsTheNewStartDateViewModel viewModel)
    {
        return await Controller.WhatIsTheNewStartDate(viewModel);
    }

    public void VerifyRedirectsToTheWhatIsTheNewEndDatePage(IActionResult result)
    {
        var redirectResult = (RedirectToRouteResult)result;

        Assert.That(redirectResult.RouteName, Is.EqualTo(RouteNames.WhatIsTheNewEndDate));

        var routeValues = redirectResult.RouteValues;

        Assert.Multiple(() =>
        {
            Assert.That(routeValues["ProviderId"], Is.EqualTo(_request.ProviderId));
            Assert.That(routeValues["ApprenticeshipHashedId"], Is.EqualTo(_request.ApprenticeshipHashedId));
            Assert.That(routeValues["AccountHashedId"], Is.EqualTo(_request.AccountHashedId));
            Assert.That(routeValues["NewStartMonth"], Is.EqualTo(_request.NewStartMonth));
            Assert.That(routeValues["NewStartYear"], Is.EqualTo(_request.NewStartYear));
            Assert.That(routeValues["NewEndMonth"], Is.EqualTo(null));
            Assert.That(routeValues["NewEndYear"], Is.EqualTo(null));
            Assert.That(routeValues["NewPrice"], Is.EqualTo(null));
        });
    }

    public void VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(IActionResult result)
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

    public WhenPostingWhatIsTheNewStartDateTestFixture SetupAdvanceToStopDate()
    {
        _request = AutoFixture.Build<ChangeOfProviderRequest>()
            .With(x => x.NewStartMonth, 1)
            .With(x => x.NewStartYear, 2020)
            .Create();

        _request.NewEndMonth = null;
        _request.NewEndYear = null;
        _request.NewPrice = null;

        MockMapper.Setup(m => m.Map<ChangeOfProviderRequest>(It.IsAny<WhatIsTheNewStartDateViewModel>()))
            .ReturnsAsync(_request);

        return this;
    }

    public WhenPostingWhatIsTheNewStartDateTestFixture SetupReturnToCheckYourAnswers()
    {
        _request = AutoFixture.Build<ChangeOfProviderRequest>()
            .With(x => x.NewStartMonth, 1)
            .With(x => x.NewStartYear, 2020)
            .With(x => x.NewEndMonth, 1)
            .With(x => x.NewEndYear, 2022)
            .With(x => x.NewPrice, 500)
            .Create();

        MockMapper.Setup(m => m.Map<ChangeOfProviderRequest>(It.IsAny<WhatIsTheNewStartDateViewModel>()))
            .ReturnsAsync(_request);

        return this;
    }
}